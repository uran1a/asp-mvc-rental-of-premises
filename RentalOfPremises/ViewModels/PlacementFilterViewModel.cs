using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentalOfPremises.ViewModels
{
    public class PlacementFilterViewModel
    {
        public List<string> SelectedCities { get; set; }
        public List<SelectListItem> AvailableCities { get; set; }
        public List<string> SelectedAreas { get; set; }
        public List<SelectListItem> AvailableAreas { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public int? MinSquare { get; set; }
        public int? MaxSquare { get; set; }
    }
}
