using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("Deal")]
    public class Deal
    {
        public int Id { get; set; }
        public DateOnly? DateOfConclusion { get; set; }
        public DateOnly StartDateRental { get; set; }
        public DateOnly EndDateRental { get; set; }
        public string TypeOfActivity { get; set; } = null!;
        public string Repairs { get; set; } = null!;
        public int RenterId { get; set; }
        public PhysicalEntity Renter { get; set; } = null!;
        public int OwnerId { get; set; }
        public PhysicalEntity Owner { get; set; } = null!;
        public int PlacementId { get; set; }
        public Placement Placement { get; set; } = null!;
    }
}
