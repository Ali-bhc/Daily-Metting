using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Daily_Metting.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        //[Required]
        //public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }=false;

        //public string ReturnUrl { get; set; } = "https://github.com/";

    }
}
