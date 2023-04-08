using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Daily_Metting.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "The password must be at least 6 characters long and contain at least one letter and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "choose User type !")]
        public bool IsAdmin { get; set; } = false;

        [Required(ErrorMessage = "choose departement !")]
        public string Departement { get; set; }
    }
}
