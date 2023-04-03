namespace Daily_Metting.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }
        public DateTime submission_time  { get; set; }= DateTime.Today;
        public User User { get; set;}
        public string status { get; set; }
        public List<Value> Values { get; set; }
        public List<Attainement> Attainements { get; set; }
    }
}
