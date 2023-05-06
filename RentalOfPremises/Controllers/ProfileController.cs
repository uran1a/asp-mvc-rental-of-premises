using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public ProfileController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Id user: " + User.Identity!.Name);
            return View(await _db.Users.Include(p => p.PhysicalEntity).Where(p => p.Id == int.Parse(User.Identity.Name!)).SingleOrDefaultAsync());
        }
    }
}

