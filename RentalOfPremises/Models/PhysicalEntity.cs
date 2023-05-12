using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("PhysicalEntity")]
    public class PhysicalEntity
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Surname { get; set; } = null!;
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [StringLength(50)]
        public string? Patronymic { get; set; } 
        public DateTime Data_Of_Birth { get; set; }
        [StringLength(50)]
        public string? Mobile_Phone { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public int Passport_Serial { get; set; }
        public int Passport_Code { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Placement> Placements { get; set; } = new();
        public List<Deal> RentalDeals { get; set; } = new();
        public List<Deal> OwnerDeals { get; set; } = new();
    }
}
