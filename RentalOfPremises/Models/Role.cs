using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<User> Users { get; set; } = new();  
        public Role()
        {
            Users = new List<User>();
        }
    }
}
