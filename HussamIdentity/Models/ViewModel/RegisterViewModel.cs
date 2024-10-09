using System.ComponentModel.DataAnnotations;

namespace HussamIdentity.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="pls write")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "pls write")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "pls write")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "write goood ")]

        public string? ConfirmPassword { get; set; }

        public string? Phone { get; set; }


    }
}
