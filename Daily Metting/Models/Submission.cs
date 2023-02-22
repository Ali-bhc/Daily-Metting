namespace Daily_Metting.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }
        public DateTime submission_time  { get; set; }= DateTime.Now;
        public User User { get; set;}
        public ICollection<Value> Values { get; set; }

        public Submission(int submissionID, DateTime submission_time, User user, ICollection<Value> values)
        {
            SubmissionID = submissionID;
            this.submission_time = submission_time;
            User = user;
            Values = values;
        }
    }
}
