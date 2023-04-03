using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface ISubmissionRepository
    {
        IEnumerable<Submission> AllSubmission { get; }
        void AddSubmission(Submission submission);
        Submission GetSubmissionByUser_Date(DateTime date, User user);
        void DeleteSubmission(Submission submission);
        List<Submission> GetUserSubmission(User user);
        List<Submission> GetUserSubmissionByPage(int page,int pagesize,User user);
        List<Submission> GetAllSubmissionByPage(int page,int pagesize);
        Submission GetUserSubmissionById(int subId);
        List<Submission> GetSubmissionsByDate(DateTime date);
        int GetStatusCountByUser(string status,User user);

        List<Submission> GetYesterdaySubmissions();

    }
}
