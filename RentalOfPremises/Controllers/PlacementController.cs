using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class PlacementController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public PlacementController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public async Task<IActionResult> Info(int id)
        {
            Console.WriteLine("Id placement: " + id);
            return  View(await _db.Placements.Include(p => p.PhysicalEntity).Where(p => p.Id == id).SingleOrDefaultAsync());
        }
        public IActionResult Conditions(int placementId, int ownerId)
        {
            Console.WriteLine(placementId + " " + ownerId);
            if(User.Identity!.Name != null)
                return View(new Deal { PlacementId = placementId, OwnerId = ownerId, RenterId = int.Parse(User.Identity!.Name)});
            return View(new Deal());
        }
        [HttpPost]
        public async Task<IActionResult> AddApplication(Deal deal)
        {
            await _db.Deals.AddAsync(deal);
            _db.SaveChanges();
            return Redirect("~/Home/Index");
        }
    }
}
