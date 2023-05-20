using RentalOfPremises.Models;
using Microsoft.EntityFrameworkCore;

namespace RentalOfPremises.Services
{
    public class DealService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PlacementService> _logger;
        public DealService(ApplicationContext dbContext, ILogger<PlacementService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<Deal>> AllDealsAsync()
        {
            return await _dbContext.Deals
                .Include(d => d.Owner)
                .Include(d => d.Renter)
                .Include(d => d.Placement).ToListAsync();
        }
        public async Task DeleteDealById(int id)
        {
           var deal = await _dbContext.Deals
                .Where(d => d.Id == id).SingleOrDefaultAsync();
            if(deal != null)
            {
                _dbContext.Deals.Remove(deal);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Delete deal with Id = {0}", deal.Id);
            }
            else
            {
                _logger.LogError("Can't find deal with id = {0}", id);
            }
        }
    }
}
