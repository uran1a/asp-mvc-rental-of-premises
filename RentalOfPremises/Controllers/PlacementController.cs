using Microsoft.AspNetCore.Mvc;

namespace RentalOfPremises.Controllers
{
    public class PlacementController : Controller
    {
        public IActionResult Info(int id)
        {
            Console.WriteLine("Id placement: " + id);
            return View();
        }
    }
}
