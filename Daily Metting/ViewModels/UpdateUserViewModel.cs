using System.ComponentModel.DataAnnotations;

namespace Daily_Metting.ViewModels
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "choose departement !")]
        public string Departement { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "choose status !")]
        public bool IsActive { get; set; }

    }
}
