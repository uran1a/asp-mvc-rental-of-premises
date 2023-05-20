using RentalOfPremises.Models;

namespace RentalOfPremises.ViewModels
{
    //tablefunctional
    public class PlacementAdminViewModel
    {
        public Dictionary<string, int> Id { get; set; }
        public Dictionary<string, string> City { get; set; }
        public Dictionary<string, string> Area { get; set; }
        public Dictionary<string, string> Street { get; set; }
        public Dictionary<string, string> House { get; set; }
        public Dictionary<string, int> Square { get; set; }
        public Dictionary<string, int> YearConstructiom { get; set; }
        public Dictionary<string, int> PhysicalEntityId { get; set; }
        public Dictionary<string, string> PhysicalEntityFullName { get; set; } 
        public Dictionary<string, int> Count { get; set; }
    }
}
