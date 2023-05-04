using System.ComponentModel.DataAnnotations.Schema;
namespace RentalOfPremises.Models
{
    [Table("PhysicalEntity")]
    public class PhysicalEntity
    {
        public int Id { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; } 
        public DateTime Data_Of_Birth { get; set; }
        public string? Mobile_Phone { get; set; }
        public string? Email { get; set; }
        public int Passport_Serial { get; set; }
        public int Passport_Code { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<Placement> Placements { get; set; } = new();
    }
}
