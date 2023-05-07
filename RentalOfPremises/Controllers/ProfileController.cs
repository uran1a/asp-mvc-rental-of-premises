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
            var currPhysicalEntity = await _db.PhysicalEntities
                            .Where(p => p.Id == int.Parse(User.Identity.Name!))
                            .SingleOrDefaultAsync();
            if(currPhysicalEntity != null)
                ViewBag.PhysicalEntity = currPhysicalEntity;
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

