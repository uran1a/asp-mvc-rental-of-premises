using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using RentalOfPremises.Models;
using RentalOfPremises.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using RentalOfPremises.Services;

namespace RentalOfPremises.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        public AccountController(ApplicationContext context, UserService userService)
        {
            _db = context;
            _userService = userService;
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
        public async Task<IActionResult> CreateUser(User model)
        {
            //Проверка на повторное нажатие
            if (model != null)
            {
                var user = _userService.UserWithLogin(model.Login);
                if (user == null)
                {
                    var newUser = await _userService.CreateUser(model);
                    await Authenticate(newUser);
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
