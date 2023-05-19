using RentalOfPremises.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
