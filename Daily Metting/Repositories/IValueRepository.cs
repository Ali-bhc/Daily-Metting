using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IValueRepository
    {
        void AddSubmissionValue(Value value);
        List<Value> GetSubmissionValue(int submissionID);
        void UpdateSubmissionValues(List<Value> values);
        void DeleteValuesBySubmission(Submission submission);
        Value GetValueBySubmissionPoint(int submissionId , Point point);
        List<Value> GetValuesByPoint_SubmissionDate(int pointId, DateTime submission_time);
        //public List<String> GetStringsValuesByPoint_SubmissionDate(int pointId, DateTime submission_time);
        public int GetSumOfValuesByPoints_SubmissionDate(int pointId, DateTime submission_time);
        
        List<string> GetCommentsConcatenations(Point point, DateTime submission_time);
        List<string> GetDescriptionsConcatenations(Point point, DateTime submission_time);
        //List<string> GetMultiValuesCommentsConcatenations(Point point, DateTime submission_time, List<string> values);
    }
}
