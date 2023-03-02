namespace Daily_Metting.Models
{
    public class Absence
    {
        public int AbsenceID { get; set; }
        public DateTime date { get; set; }= DateTime.Now;
        public string Status { get; set; }
        public User? User { get; set; }

    }
}
