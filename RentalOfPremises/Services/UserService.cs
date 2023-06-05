    using RentalOfPremises.Models;
using Microsoft.EntityFrameworkCore;

namespace RentalOfPremises.Services
{
    public class UserService
    {
        private readonly ApplicationContext _dbContext;
        private readonly ILogger<PlacementService> _logger;
        public UserService(ApplicationContext dbContext, ILogger<PlacementService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<User>> AllUsers()
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
        public async Task<User> UserById(int id)
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
        public async Task<User> UserWithLogin(string login)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user != null)
                _logger.LogInformation("Get users with login = {0}", login);
            else
                _logger.LogError("Can't get user with login = {0}", login);
            return user;
        }
        public async Task DeleteUserById(int id)
        {
            int numberRow = await _dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
            Console.WriteLine(numberRow);
        }
        public async Task UpdateUser(User user)
        {
            await _dbContext.Users
               .Where(u => u.Id == user.Id)
               .ExecuteUpdateAsync(s => s
                   .SetProperty(u => u.Login, p => user.Login)
                   .SetProperty(u => u.Password, p => user.Password));
            await _dbContext.PhysicalEntities
                .Where(p => p.Id == user.PhysicalEntity.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Name, p => user.PhysicalEntity.Name)
                    .SetProperty(p => p.Surname, p => user.PhysicalEntity.Surname)
                    .SetProperty(p => p.Patronymic, p => user.PhysicalEntity.Patronymic)
                    .SetProperty(p => p.Data_Of_Birth, p => user.PhysicalEntity.Data_Of_Birth)
                    .SetProperty(p => p.Mobile_Phone, p => user.PhysicalEntity.Mobile_Phone)
                    .SetProperty(p => p.Email, p => user.PhysicalEntity.Email)
                    .SetProperty(p => p.Passport_Serial, p => user.PhysicalEntity.Passport_Serial)
                    .SetProperty(p => p.Passport_Code, p => user.PhysicalEntity.Passport_Code));
            _logger.LogInformation("Update user with id = {0}", user.Id);
        }
        public async Task<User> CreateUser(User user)
        {
            Role? userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "user");
            if (userRole != null)
                user!.Role = userRole;
            var newUser = _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            if (newUser != null)
            {
                _logger.LogInformation("Create user with id = {0}", newUser.Entity.Id);
                return newUser.Entity;
            }
            return null;
        }
        
    }
}
