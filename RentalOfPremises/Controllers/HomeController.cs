using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;

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
        
        public async Task<IActionResult> Index()
        {
            return View(await _db.Placements.Where(p => p.PhysicalEntityId != int.Parse(User.Identity!.Name!)).ToListAsync());
        }
        public async Task<IActionResult> Messages()
        {
            var messages = await _db.Deals
                .Include(d => d.Owner)
                .Include(d => d.Renter)
                .Include(d => d.Placement)
                .Where(d => d.DateOfConclusion == null)
                .Where(d => d.OwnerId == int.Parse(User.Identity!.Name!)).ToListAsync();
            return View(messages);
        }
        public async Task<IActionResult> ConcludeDeal(int dealId)
        {
            Deal? deal = await _db.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
            if (deal != null)
            {
                deal.DateOfConclusion = DateTime.Now;
                _db.SaveChanges();
            }
            return Redirect("~/Home/Messages");
        }
        public async Task<IActionResult> DeleteDeal(int dealId)
        {
            Deal? deal = await _db.Deals.FirstOrDefaultAsync(d => d.Id == dealId);
            if (deal != null)
            {
                _db.Deals.Remove(deal);
                _db.SaveChanges();
            }
            return Redirect("~/Home/Messages");
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