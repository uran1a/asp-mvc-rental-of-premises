using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("Role")]
    public class Role
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Required]
        public virtual List<User> Users { get; set; } = null!;
    }
}
