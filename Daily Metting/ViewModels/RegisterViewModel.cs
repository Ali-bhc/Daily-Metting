using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Daily_Metting.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        public string Departement { get; set; }
    }
}
