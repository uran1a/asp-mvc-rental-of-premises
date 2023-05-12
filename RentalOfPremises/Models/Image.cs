using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("Image")]
    public class Image
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; } = null!;
        public byte[] Content { get; set; } = null!;
        public int PlacementId { get; set; }
        public virtual Placement Placement { get; set; } = null!; 
    }
}
