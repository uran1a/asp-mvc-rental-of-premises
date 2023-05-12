using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;

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
        public IActionResult Create()
        {
            ViewBag.UserId = User.Identity!.Name!;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Placement model, List<IFormFile> images)
        {
           if(model != null)
            {
                var placement = _db.Placements.Add(model);
                await _db.SaveChangesAsync();
                if (model.Images != null)
                {
                    List<Image> placementImages = new List<Image>();
                    foreach (var upload in images)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(upload.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)upload.Length);
                        }
                        placementImages.Add(new Image
                        {
                            ContentType = upload.ContentType,
                            Content = imageData,
                            PlacementId = placement.Entity.Id
                        });
                    }
                    for (int i = 0; i < placementImages.Count; i++)
                    {
                        var image = _db.Images.Add(placementImages[i]);
                        await _db.SaveChangesAsync();
                        if (i == 0)
                        {
                            _db.Placements
                               .Where(p => p.Id == placement.Entity.Id)
                               .ExecuteUpdate(s => s.SetProperty(p => p.Preriew_Image_Id, p => image.Entity.Id));
                        }
                    }
                }
            }
            return Redirect("~/Profile/Index");
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
