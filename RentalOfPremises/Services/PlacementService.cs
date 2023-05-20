using RentalOfPremises.Models;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.ViewModels;
using System.Linq;

namespace RentalOfPremises.Services
{
    public class PlacementService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PlacementService> _logger;
        public PlacementService(ApplicationContext dbContext, ILogger<PlacementService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<Placement>> AllPlacementsAsync()
        {
            return await _dbContext.Placements
                .Include(p => p.Images)
                .Include(p => p.PhysicalEntity)
                .ToListAsync();
        }
        public async Task<List<PlacementAdminViewModel>> AllPlacementsAdminViewModelAsync()
        {
            List<PlacementAdminViewModel> toReturn = new();
            var allPlacements = await _dbContext.Placements
                .Include(p => p.Images)
                .Include(p => p.PhysicalEntity)
                .ToListAsync();
            foreach(var placement in allPlacements)
            {
                toReturn.Add(new PlacementAdminViewModel
                {
                    Id = new Dictionary<string, int>() { { "Id", placement.Id } },
                    City = new Dictionary<string, string>() { { "Город", placement.City } },
                    Area = new Dictionary<string, string>() { { "Район", placement.Area } },
                    Street = new Dictionary<string, string>() { { "Улица", placement.Street } },
                    House = new Dictionary<string, string>() { { "Дом", placement.House } },
                    Square = new Dictionary<string, int>() { { "Площадь", placement.Square } },
                    YearConstructiom = new Dictionary<string, int>() { { "Id владельца", placement.PhysicalEntityId } },
                    PhysicalEntityId = new Dictionary<string, int>() { { "Дата постройки", placement.YearConstruction } },
                    PhysicalEntityFullName = new Dictionary<string, string>() { { "Владелец", placement.PhysicalEntity.Surname + " " + placement.PhysicalEntity.Name + " " + placement.PhysicalEntity.Patronymic } }
                });
            }
            return toReturn;
        }
        public async Task<Placement> PlacementFindByIdAsync(int id)
        {
            var placement = await _dbContext.Placements
                .Include(p => p.Images)
                .Include(p => p.PhysicalEntity)
                .Include(p => p.Deal)
                    .ThenInclude(d => d.Renter)
                .Where(p => p.Id == id).SingleOrDefaultAsync();
            if (placement != null)
                _logger.LogInformation("Get placement with Id = {0}", id);
            else
                _logger.LogError("Can't get placement with Id = {0}", id);
            return placement!;
        }
        public async Task AddPlacementWihtImagesAsync(Placement p, List<IFormFile> images)
        {
            if (p != null)
            {
                var placement = await _dbContext.AddAsync(p);
                await _dbContext.SaveChangesAsync();
                if (p.Images != null)
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
                    _dbContext.Images.AddRange(placementImages);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("Add placement with id = {0}", placement.Entity.Id);
                }
                else
                {
                    _logger.LogError("You need add photo to placement");
                }
            }
            _logger.LogError("Placement is empty");
        }
        public async Task UpdatePlacementAsync(Placement placement, List<IFormFile?> images)
        {
            if (placement != null)
            {
                await _dbContext.Placements
                    .Where(p => p.Id == placement.Id)
                    .ExecuteUpdateAsync(s => s
                        .SetProperty(p => p.City, p => placement.City)
                        .SetProperty(p => p.Area, p => placement.Area)
                        .SetProperty(p => p.Street, p => placement.Street)
                        .SetProperty(p => p.House, p => placement.House)
                        .SetProperty(p => p.Square, p => placement.Square)
                        .SetProperty(p => p.Price, p => placement.Price)
                        .SetProperty(p => p.Description, p => placement.Description));
                for (int i = 0; i < images.Count; i++)
                {
                    if (images[i] != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(images[i].OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)images[i].Length);
                        }
                        var image = await _dbContext.Images
                            .Where(img => img.PlacementId == placement.Id)
                            .Where(img => img.FileName.Equals("image" + i)).SingleOrDefaultAsync();
                        if(image != null)
                        {
                            await _dbContext.Images
                                .Where(img => img.Id == image.Id)
                                .ExecuteUpdateAsync(s => s
                                    .SetProperty(img => img.Content, img => imageData)
                                    .SetProperty(img => img.ContentType, img => images[i].ContentType));
                            _logger.LogInformation("Update imge with Id = {0}", image.Id);
                        }
                        else
                        {
                            var newImage = await _dbContext.Images
                                .AddAsync(new Image
                                {
                                    FileName = "image" + i,
                                    ContentType = images[i].ContentType,
                                    Content = imageData,
                                    PlacementId = placement.Id
                                });
                            await _dbContext.SaveChangesAsync();
                            _logger.LogInformation("Add image with Id = {0}", newImage.Entity.Id);
                        }
                    }
                }
                _logger.LogInformation("Update placement with Id = {0}", placement.Id);
            }
            _logger.LogError("Placement is empty");
        }
        public async Task DeletePlacementByIdAsync(int id)
        {
            var placement = await _dbContext.Placements
                .Where(p => p.Id == id).SingleOrDefaultAsync();
            _dbContext.Placements.Remove(placement!);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Delete placement with Id = {0}", placement.Id);
        }
        public async Task<List<PlacementAdminViewModel>> CountUserPlacementsAsync(string condition, int count)
        {
            List<PlacementAdminViewModel> availablePlacements = await _dbContext.Placements
                 .GroupBy(p => p.PhysicalEntityId)
                 .Select(p => new PlacementAdminViewModel
                 {
                     PhysicalEntityId = new Dictionary<string, int>() { { "Id", p.Key } },
                     Count = new Dictionary<string, int>() { { "Кол-во помещений", p.Count() } },
                 }).ToListAsync();
            foreach(var availablePlacement in availablePlacements)
            {
               var placement = await _dbContext.PhysicalEntities
                    .Where(ph => ph.Id == availablePlacement.PhysicalEntityId.Values.First()).SingleOrDefaultAsync();
                if(placement != null)
                    availablePlacement.PhysicalEntityFullName = new Dictionary<string, string>() { { "Владелец", placement.Surname + " " + placement.Name + " " + placement.Patronymic } };
            }
            if (condition.Equals(">"))
                availablePlacements = availablePlacements.Where(p => p.Count.Values.First() > count).ToList();
            else if (condition.Equals("<"))
                availablePlacements = availablePlacements.Where(p => p.Count.Values.First() < count).ToList();
            return availablePlacements;
        }
        public async Task<List<PlacementAdminViewModel>> CountCityPlacementsAsync(string condition, int count)
        {

            List<PlacementAdminViewModel> availablePlacements = await _dbContext.Placements
                   .GroupBy(p => p.City)
                   .Select(p => new PlacementAdminViewModel
                   { 
                       City = new Dictionary<string, string>() { { "Город", p.Key } },
                       Count = new Dictionary<string, int>() { { "Кол-во помещений", p.Count() } },
                   }).ToListAsync();
            if (condition.Equals(">"))
                availablePlacements = availablePlacements.Where(p => p.Count.Values.First() > count).ToList();
            else if (condition.Equals("<"))
                availablePlacements = availablePlacements.Where(p => p.Count.Values.First() < count).ToList();
            return availablePlacements;
        }
    }
}
