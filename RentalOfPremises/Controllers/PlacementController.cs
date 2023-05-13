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
            var placement = await _placementService.GetPlacementFindByIdAsync(id);
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
        public async Task<IActionResult> Create(Placement model, List<IFormFile> images)
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
        public async Task<IActionResult> Update(Placement model, IFormFile image1, IFormFile image2, IFormFile image3)
        {
            if(model != null)
            {
                await _db.Placements
                    .Where(p => p.Id == model.Id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.City, p => model.City)
                        .SetProperty(p => p.Area, p => model.Area)
                        .SetProperty(p => p.Street, p => model.Street)
                        .SetProperty(p => p.House, p => model.House)
                        .SetProperty(p => p.Square, p => model.Square)
                        .SetProperty(p => p.Price, p => model.Price)
                        .SetProperty(p => p.Description, p => model.Description));
                 List<IFormFile?> images = new List<IFormFile?> { image1, image2, image3 };
                for(int i = 0; i < images.Count; i++)
                {
                    if (images[i] != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(images[i].OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)images[i].Length);
                        }
                        var image = await _db.Images
                            .Where(i => i.PlacementId == model.Id)
                            .Where(i => i.FileName.Equals("image0"))
                            .ExecuteUpdateAsync(i => i
                                .SetProperty(i => i.Content, i => imageData)
                                .SetProperty(i => i.ContentType, i => image1.ContentType));
                    }
                }
                
            }
            return Redirect("~/Profile/Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var placement = await _db.Placements
                .Where(p => p.Id == id).SingleOrDefaultAsync();
            _db.Placements.Remove(placement!);
            await _db.SaveChangesAsync();
            return Redirect("~/Profile/Index");
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
