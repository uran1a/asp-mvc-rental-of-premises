using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("User")]
    public class User
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Login { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string Password { get; set; } = null!;
        [Required]
        public PhysicalEntity PhysicalEntity { get; set; } = null!;
        [Required]
        public int RoleId { get; set; }
        [Required]
        public Role Role { get; set; } = null!;
    }
}