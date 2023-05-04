using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public PhysicalEntity PhysicalEntity { get; set; } = null!;
        public int? RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}