﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RentalOfPremises.Models;
using System.Diagnostics;

namespace RentalOfPremises.Controllers
{
    [Authorize]
    public class PlacementController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public PlacementController(ApplicationContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public async Task<IActionResult> Info(int id)
        {
            Console.WriteLine("Id placement: " + id);
            return  View(await _db.Placements.Include(p => p.PhysicalEntity).Where(p => p.Id == id).SingleOrDefaultAsync());
        }
    }
}
