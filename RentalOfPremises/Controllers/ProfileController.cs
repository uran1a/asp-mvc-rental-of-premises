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
        public async Task<IActionResult> Update(PhysicalEntity model)
        {
            if (model == null) return RedirectToAction("Index");
            await _db.PhysicalEntities
                            .Where(p => p.Id == int.Parse(User.Identity!.Name!))
                            .ExecuteUpdateAsync(s => s
                                .SetProperty(p => p.Name, p => model.Name)
                                .SetProperty(p => p.Surname, p => model.Surname)
                                //.SetProperty(p => p.Patronymic, u => model.Patronymic == null ? "" : model.Patronymic)
                                .SetProperty(p => p.Patronymic, p => model.Patronymic)
                                .SetProperty(p => p.Data_Of_Birth, p => model.Data_Of_Birth)
                                .SetProperty(p => p.Mobile_Phone, p => model.Mobile_Phone)
                                .SetProperty(p => p.Email, p => model.Email)
                                .SetProperty(p => p.Passport_Serial, p => model.Passport_Serial)
                                .SetProperty(p => p.Passport_Code, p => model.Passport_Code));
            return RedirectToAction("Index");
        }
        
    }
}

