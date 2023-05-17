using RentalOfPremises.Models;
using Microsoft.EntityFrameworkCore;

namespace RentalOfPremises.Services
{
    public class AdminService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PlacementService> _logger;
        public AdminService(ApplicationContext dbContext, ILogger<PlacementService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Include(u => u.PhysicalEntity)
                .Where(u => u.RoleId != 1).ToListAsync();
            if (users != null)
                _logger.LogInformation("Get all users without admin");
            else
                _logger.LogError("Can't get all users without admin");
            return users!;
        }
        public async Task<User> GetUserById(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.PhysicalEntity)
                .Where(u => u.Id == id).SingleOrDefaultAsync();
            if (user != null)
                _logger.LogInformation("Get users by id = {0}", user.Id);
            else
                _logger.LogError("Can't get user by id = {0}", id);
            return user!;
        }
    }
}
