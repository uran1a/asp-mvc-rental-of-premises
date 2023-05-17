using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentalOfPremises.Services;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            return View(await _adminService.GetAllUsers());
        }
        public async Task<IActionResult> UpdateUser(int id)
        {
            ViewBag.Layout = "/View/Admin/_Layout.cshtml";
            return View("PhysicalEntityPartial", _adminService.GetUserById(id).Result.PhysicalEntity);
        }
    }
}
