using System.ComponentModel.DataAnnotations;

namespace HussamIdentity.Models.ViewModel
{
    public class CreateRoleModelView
    {
        [Required]
        public string? RoleName { get; set; }
    }
}
