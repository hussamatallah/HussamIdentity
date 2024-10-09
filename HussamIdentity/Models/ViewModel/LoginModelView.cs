using System.ComponentModel.DataAnnotations;

namespace HussamIdentity.Models.ViewModel
{
    public class LoginModelView
    {
        [Required(ErrorMessage = "pls write")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "pls write")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
