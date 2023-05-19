using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalOfPremises.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly UserService _userService;
        private readonly PlacementService _placementService;
        public AdminController(UserService userService, PlacementService placementService)
        {
            _userService = userService;
            _placementService = placementService;
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            return View(await _userService.AllUsers());
        }
        
        public IActionResult CreateUser()
        {
            ViewBag.Layout = "/View/Admin/_Layout.cshtml";
            ViewData["Controller"] = "Admin";
            return View("Create/UserPartial", new User());
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(User model)
        {
            if (model != null)
            {
                var user = await _userService.UserWithLogin(model.Login);
                if (user == null)
                {
                    var newUser = await _userService.CreateUser(model);
                    return Redirect("~/Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логин уже существует");
                }
            }
            return Redirect("~/Admin/CreateUser");
        }
        public async Task<IActionResult> UpdateUser(int id)
        {
            ViewBag.Layout = "/View/Admin/_Layout.cshtml";
            ViewData["Controller"] = "Admin";
            return View("Update/UserPartial", await _userService.UserById(id));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model)
        {
            if (model == null) 
                return RedirectToAction("Index");
            else
                await _userService.UpdateUser(model);
            return Redirect("~/Admin");
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserById(id);
            return Redirect("~/Admin");
        }
        [HttpGet]
        public async Task<IActionResult> Placements()
        {
            return View(await _placementService.AllPlacementsAsync());
        }
        public IActionResult CreatePlacement()
        {
            ViewBag.Layout = "/View/Admin/_Layout.cshtml";
            ViewData["Controller"] = "Admin";
            return View("Create/PlacementPartial", new Placement());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlacement(Placement model, List<IFormFile> images)
        {
            await _placementService.AddPlacementWihtImagesAsync(model, images);
            return Redirect("~/Admin");
        }
        public async Task<IActionResult> UpdatePlacement(int id)
        {
            ViewBag.Layout = "/View/Admin/_Layout.cshtml";
            ViewData["Controller"] = "Admin";
            return View("Update/PlacementPartial", await _placementService.PlacementFindByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlacement(Placement model, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            await _placementService.UpdatePlacementAsync(model, new List<IFormFile?> { image1, image2, image3 });
            return Redirect("~/Admin");
        }
        public async Task<IActionResult> DeletePlacement(int id)
        {
            await _placementService.DeletePlacementByIdAsync(id);
            return Redirect("~/Admin");
        }
    }
}
