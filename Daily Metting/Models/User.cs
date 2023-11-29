using Microsoft.AspNetCore.Identity;

namespace Daily_Metting.Models
{

    public class User : IdentityUser
    {
        public string Name { get; set; }
      
        public bool IsAdmin { get; set; }
        public string Departement { get; set; }
        public int MissedSubmissions { get; set; } = 0;
        public List<Submission>? Submissions { get; set; }
        public List<Absence>? Absences { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
