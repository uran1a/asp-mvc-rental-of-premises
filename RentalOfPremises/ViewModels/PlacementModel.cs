namespace RentalOfPremises.ViewModels
{
    public class PlacementModel
    {
       public string? City { get; set; } 
        public string? Area { get; set; } 
        public string? Street { get; set; }
        public string? House { get; set; } 
        public int Square { get; set; }
        public DateTime Date_Of_Construction { get; set; }
        public int PhysicalEntityId { get; set; }
    }
}
