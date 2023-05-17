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
        public async Task<IActionResult> Update(User model)
        {
            if (model == null) return RedirectToAction("Index");
            await _db.Users
                .Where(u => u.Id == model.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Login, p => model.Login)
                    .SetProperty(u => u.Password, p => model.Password));
            await _db.PhysicalEntities
                .Where(p => p.Id == model.PhysicalEntity.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Name, p => model.PhysicalEntity.Name)
                    .SetProperty(p => p.Surname, p => model.PhysicalEntity.Surname)
                    //.SetProperty(p => p.Patronymic, u => model.Patronymic == null ? "" : model.Patronymic)
                    .SetProperty(p => p.Patronymic, p => model.PhysicalEntity.Patronymic)
                    .SetProperty(p => p.Data_Of_Birth, p => model.PhysicalEntity.Data_Of_Birth)
                    .SetProperty(p => p.Mobile_Phone, p => model.PhysicalEntity.Mobile_Phone)
                    .SetProperty(p => p.Email, p => model.PhysicalEntity.Email)
                    .SetProperty(p => p.Passport_Serial, p => model.PhysicalEntity.Passport_Serial)
                    .SetProperty(p => p.Passport_Code, p => model.PhysicalEntity.Passport_Code));
            return RedirectToAction("Index");
        }
        
    }
}

