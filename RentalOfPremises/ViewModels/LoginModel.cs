using System.ComponentModel.DataAnnotations;
namespace RentalOfPremises.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Login")]
        public string Login { get; set; } = "";
        [Required(ErrorMessage = "Не указан Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
