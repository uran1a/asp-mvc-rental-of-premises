using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("Deal")]
    public class Deal
    {
        public int Id { get; set; }
        public DateTime? DateOfConclusion { get; set; }
        public DateTime StartDateRental { get; set; }
        public DateTime EndDateRental { get; set; }
        public string TypeOfActivity { get; set; } = null!;
        public string Repairs { get; set; } = null!;
        public int RenterId { get; set; }
        public PhysicalEntity Renter { get; set; } = null!;
        public int OwnerId { get; set; }
        public PhysicalEntity Owner { get; set; } = null!;
        public int PlacementId { get; set; }
        [Required]
        public Placement Placement { get; set; } = null!;
    }
}
