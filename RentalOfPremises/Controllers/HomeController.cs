using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;
using RentalOfPremises.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        
        //public async Task<IActionResult> Index(List<string>? Cities, List<CheckboxViewModel>? Areas)
        public async Task<IActionResult> Index(PlacementFilterViewModel model)
        {
            IQueryable<Placement> placements = _db.Placements
                .Include(p => p.Images);
            if(model != null)
            {
                if(model.SelectedCities != null && model.SelectedCities.Count > 0)
                {
                    placements = placements.Where(p => model.SelectedCities.Contains(p.City));
                }
                if (model.SelectedAreas != null && model.SelectedAreas.Count > 0)
                {
                    placements = placements.Where(p => model.SelectedAreas.Contains(p.Area));
                }
                if (model.MinPrice != null)
                {
                    placements = placements.Where(p => p.Price > model.MinPrice);
                }
                if (model.MaxPrice != null)
                {
                    placements = placements.Where(p => p.Price < model.MaxPrice);
                }
                if (model.MinSquare != null)
                {
                    placements = placements.Where(p => p.Square > model.MinSquare);
                }
                if (model.MaxSquare != null)
                {
                    placements = placements.Where(p => p.Square < model.MaxSquare);
                }
            }
            placements = placements
                .Where(p => p.Deal == null)
                .Where(p => p.PhysicalEntityId != int.Parse(User.Identity!.Name!));
            var filter = _db.Placements
                .Where(p => p.Deal == null)
                .Where(p => p.PhysicalEntityId != int.Parse(User.Identity!.Name!));
            ViewBag.Filter = new PlacementFilterViewModel
            {
                SelectedCities = model.SelectedCities,
                AvailableCities = GetSelectListItem(filter.Select(p => p.City).Distinct().ToList()),
                SelectedAreas = model.SelectedAreas,
                AvailableAreas = GetSelectListItem(filter.Select(p => p.Area).Distinct().ToList()),
                MinPrice = model.MinPrice,
                MaxPrice = model.MaxPrice,
                MinSquare = model.MinSquare,
                MaxSquare = model.MaxSquare
            };
            return View(placements);
        }
        private List<SelectListItem> GetSelectListItem(List<string> values)
        {
            List<SelectListItem> ToReturn = new List<SelectListItem>();
            foreach(var item in values)
                ToReturn.Add(new SelectListItem { Text = item, Value = item });
            return ToReturn;
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