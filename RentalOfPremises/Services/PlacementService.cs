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
        public async Task<Placement> GetPlacementFindByIdAsync(int id)
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
    }
}
