using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using System.Web;
using RentalOfPremises.Services;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        public ProfileController(ApplicationContext context, ILogger<HomeController> logger, UserService userService)
        {
            _db = context;
            _logger = logger;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Id user: " + User.Identity!.Name);
            var user = await _db.Users
                            .Include(u => u.PhysicalEntity)
                            .Where(p => p.PhysicalEntity.Id == int.Parse(User.Identity.Name!))
                            .SingleOrDefaultAsync();
            if(user != null)
                ViewBag.User = user;
            var placements = await _db.Placements
                           .Include(p => p.Deal)
                           .Where(p => p.PhysicalEntityId == int.Parse(User.Identity.Name!))
                           .ToListAsync();
            if (placements != null)
                ViewBag.Placements = placements;
            var renterPlacements = await _db.Deals
                            .Include(d => d.Owner)
                            .Include(d => d.Placement)
                            .Where(d => d.DateOfConclusion != null)
                            .Where(d => d.RenterId == int.Parse(User.Identity.Name!))
                            .ToListAsync();
            if (placements != null)
                ViewBag.RenterPlacements = renterPlacements;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model)
        {
            if (model == null) 
                return RedirectToAction("Index");
            else 
                await _userService.UpdateUser(model);
            return RedirectToAction("Index");
        }
    }
}

