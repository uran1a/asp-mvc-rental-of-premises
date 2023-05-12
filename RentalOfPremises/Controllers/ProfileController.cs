using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using System.Web;

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
            var physicalEntity = await _db.PhysicalEntities
                            .Where(p => p.Id == int.Parse(User.Identity.Name!))
                            .SingleOrDefaultAsync();
            if(physicalEntity != null)
                ViewBag.PhysicalEntity = physicalEntity;
            var placements = await _db.Placements
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
        public async Task<IActionResult> Update(PhysicalEntity currPhysicalEntity)
        {
            if(currPhysicalEntity == null) return RedirectToAction("Index");
            var physicalEntity = await _db.PhysicalEntities
                            .Where(p => p.Id == int.Parse(User.Identity!.Name!))
                            .SingleOrDefaultAsync();
            if(physicalEntity != null)
            {
                if (!physicalEntity.Name.Equals(currPhysicalEntity!.Name))
                    physicalEntity.Name = currPhysicalEntity.Name;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        
    }
}

