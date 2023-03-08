using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IValueRepository
    {
        void AddSubmissionValue(Value value);
        List<Value> GetSubmissionValue(int submissionID);
        void UpdateSubmissionValues(List<Value> values);
        void DeleteValuesBySubmission(Submission submission);
    }
}
