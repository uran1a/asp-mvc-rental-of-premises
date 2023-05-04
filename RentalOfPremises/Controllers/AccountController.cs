using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RentalOfPremises.Models;
using RentalOfPremises.ViewModels;
using System.Diagnostics;
using System.Security.Claims;

namespace RentalOfPremises.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _db;
        public AccountController(ApplicationContext context)
        {
            _db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users
                    .Include(u => u.PhysicalEntity)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                if(user != null)
                {
                    await Authenticate(user);
                    //Добавить enum
                    if (user.Role.Name == "user")
                        return RedirectToAction("Index", "Home");
                    else if (user.Role.Name == "admin")
                        return RedirectToAction("Index", "Admin");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //Проверка на повторное нажатие
            if (ModelState.IsValid)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    var newUser = new Models.User
                    {
                        Login = model.Login,
                        Password = model.Password,
                        PhysicalEntity = new PhysicalEntity
                        {
                            Name = model.Name,
                            Surname = model.Surname,
                            Patronymic = model.Patronymic,
                            Data_Of_Birth = model.Data_Of_Birth,
                            Mobile_Phone = model.Mobile_Phone,
                            Email = model.Email,
                            Passport_Code = model.Passport_Code,
                            Passport_Serial = model.Passport_Serial
                        }
                    };
                    Role? userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        newUser!.Role = userRole;
                    var addedUser = _db.Users.Add(newUser);
                    await _db.SaveChangesAsync();
                    Console.WriteLine("Id added user: " + addedUser.Entity.Id);
                    await Authenticate(addedUser.Entity);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логин уже существует");
                }
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.PhysicalEntity.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
