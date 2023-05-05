using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated) 
                Console.WriteLine(User.Identity.Name);
            return View(await _db.Placements.Where(p => p.PhysicalEntityId != int.Parse(User.Identity.Name!)).ToListAsync());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}