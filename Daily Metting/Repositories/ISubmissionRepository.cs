using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface ISubmissionRepository
    {
        IEnumerable<Submission> AllSubmission { get; }
        void AddSubmission(Submission submission,List<Value> ValuesList);
    }
}
