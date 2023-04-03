using Microsoft.AspNetCore.Identity;

namespace Daily_Metting.Models
{

    public class User : IdentityUser
    {
        //public int UserID { get; set; }
        public string Name { get; set; }
        //public string Email { get; set; }
        //public string UserName { get; set; }
        //public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Departement { get; set; }
        //public DateTime Created { get; set; } = DateTime.Now;
        //public DateTime LastUpdated { get; set; }
        public int MissedSubmissions { get; set; } = 0;
        public List<Submission>? Submissions { get; set; }
        public List<Absence>? Absences { get; set; }



    }
}
