using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalOfPremises.Models
{
    [Table("Placement")]
    public class Placement
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Area { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Street { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string House { get; set; } = null!;
        [Required]
        public int Square { get; set; }
        [Required]
        public int YearConstruction { get; set; }
        [Required]
        public int Price { get; set; }
        public string? Description { get; set; }
        [Required]
        public int PhysicalEntityId { get; set; }
        [Required]
        public PhysicalEntity PhysicalEntity { get; set; } = null!;
        public Deal? Deal { get; set; }
        public virtual List<Image>? Images { get; set; }
    }
}
