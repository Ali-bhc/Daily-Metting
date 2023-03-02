namespace Daily_Metting.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }
        public DateTime submission_time  { get; set; }= DateTime.Now;
        public User User { get; set;}
        public List<Value> Values { get; set; }
    }
}
