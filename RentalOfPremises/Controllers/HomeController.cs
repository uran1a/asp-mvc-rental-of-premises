using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public HomeController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            /*User u1 = new User { Login = "u1", Password = "123" };
            User u2 = new User { Login = "u2", Password = "123" };
            _db.Users.AddRange(u1, u2);

            PhysicalEntity pe1 = new PhysicalEntity
            {
                Name = "Никита",
                Surname = "Забурдяев",
                Patronymic = "Сергеевич",
                Data_Of_Birth = new DateTime(2003, 9, 26),
                Mobile_Phone = "+79204097999",
                Email = "9797171z@mail.ru",
                Passport_Serial = 123123,
                Passport_Code = 123,
                User = u1
            };
            PhysicalEntity pe2 = new PhysicalEntity
            {
                Name = "Владислав",
                Surname = "Забурдяев",
                Data_Of_Birth = new DateTime(2003, 9, 25),
                Mobile_Phone = "+79204097999",
                Passport_Serial = 123123,
                Passport_Code = 123,
                User = u2
            };
            _db.PhysicalEntities.AddRange(pe1, pe2);

            Placement p1 = new Placement { 
                City = "Воронеж", 
                Area = "Северный", 
                Street = "Шишкова", 
                House = "145", 
                Square = 75, 
                Owner = pe1, 
                Date_Of_Construction = new DateTime(2015, 5, 6) 
            };
            Placement p2 = new Placement
            {
                City = "Воронеж",
                Area = "Центральный",
                Street = "Шишкова",
                House = "145",
                Square = 75,
                Owner = pe1,
                Date_Of_Construction = new DateTime(2015, 5, 6)
            };
            Placement p3 = new Placement
            {
                City = "Москва",
                Area = "Северный",
                Street = "Шишкова",
                House = "124",
                Square = 56,
                Owner = pe2,
                Date_Of_Construction = new DateTime(2017, 3, 18)
            };
            _db.Placements.AddRange(p1, p2, p3);

            _db.SaveChanges();

            foreach(var user in _db.Users.Include(u => u.Physical_Entity).ToList())
            {
                Console.WriteLine($"{user.Physical_Entity?.Name} {user.Physical_Entity?.Surname} {user.Login} {user.Password}");
                if (user.Physical_Entity?.Placements != null)
                    foreach (var placement in user.Physical_Entity.Placements)
                        Console.WriteLine($"{placement.City} {placement.Area} {placement.Street}");
            }*/
            if (User.Identity!.IsAuthenticated) 
                Console.WriteLine(User.Identity.Name);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}