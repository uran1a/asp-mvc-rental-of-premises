using System.ComponentModel.DataAnnotations;
namespace RentalOfPremises.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Не указан \"Фамилия\"")]
        public string Surname { get; set; } = null!;
        [Required(ErrorMessage = "Не указан \"Имя\"")]
        public string Name { get; set; } = null!;
        public string? Patronymic { get; set; }
        [Required(ErrorMessage = "Не указан \"Дата рождения\"")]
        public DateTime Data_Of_Birth { get; set; }
        public string? Mobile_Phone { get; set; }
        public string? Email { get; set; }
        [Required(ErrorMessage = "Не указан \"Серия паспорта\"")]
        public int Passport_Serial { get; set; }
        [Required(ErrorMessage = "Не указан \"Код паспорта\"")]
        public int Passport_Code { get; set; }
    }
}
