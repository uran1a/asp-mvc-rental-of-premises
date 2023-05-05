using System.ComponentModel.DataAnnotations.Schema;

namespace RentalOfPremises.Models
{
    [Table("Placement")]
    public class Placement
    {
        public long Id { get; set; }
        public string City { get; set; } = null!;
        public string Area { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string House { get; set; } = null!;
        public int Square { get; set; }
        public DateTime Date_Of_Construction { get; set; }
        public long Preriew_Image_Id { get; set; }
        public int PhysicalEntityId { get; set; }
        public PhysicalEntity PhysicalEntity { get; set; } = null!;
    }
}
