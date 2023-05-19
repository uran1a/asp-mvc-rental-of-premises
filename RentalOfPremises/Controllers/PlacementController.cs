using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;
using RentalOfPremises.Services;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class PlacementController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;
        private readonly PlacementService _placementService;

        public PlacementController(ApplicationContext context, ILogger<HomeController> logger, PlacementService placementService)
        {
            _db = context;
            _logger = logger;
            _placementService = placementService;
        }
        public async Task<IActionResult> Index(int id)
        {
            Console.WriteLine("Id placement: " + id);
            var placement = await _placementService.PlacementFindByIdAsync(id);
            if (placement.PhysicalEntityId == int.Parse(User.Identity!.Name!))
                ViewBag.IsOwner = true;
            else 
                ViewBag.IsOwner = false;
            return View(placement);
        }
        public IActionResult Create()
        {
            ViewBag.UserId = User.Identity!.Name!;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlacement(Placement model, List<IFormFile> images)
        {
            if (model != null)
            {
                var placement = _db.Placements.Add(model);
                await _db.SaveChangesAsync();
                if (model.Images != null)
                {
                    List<Image> placementImages = new List<Image>();
                    for (int i = 0; i < images.Count; i++)
                    {

                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(images[i].OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)images[i].Length);
                        }
                        placementImages.Add(new Image
                        {
                            FileName = "image" + i,
                            ContentType = images[i].ContentType,
                            Content = imageData,
                            PlacementId = placement.Entity.Id
                        });
                    }
                    _db.Images.AddRange(placementImages);
                    await _db.SaveChangesAsync();
                }
            }
            return Redirect("~/Profile/Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            return View(await _db.Placements
                .Include(p => p.Images)
                .Where(p => p.Id == id).SingleOrDefaultAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlacement(Placement model, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            await _placementService.UpdatePlacementAsync(model, new List<IFormFile?> { image1, image2, image3 });
            return Redirect("~/Profile/Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _placementService.DeletePlacementByIdAsync(id);
            return Redirect("~/Profile/Index");
        }

        private void DeletePlacementByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Conditions(int placementId, int ownerId)
        {
            if(User.Identity!.Name != null)
            {
                return View(new Deal
                {
                    PlacementId = placementId,
                    OwnerId = ownerId,
                    RenterId = int.Parse(User.Identity!.Name)
                });
            }
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
