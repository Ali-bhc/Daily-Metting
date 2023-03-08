using Daily_Metting.DAO;
using Daily_Metting.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Daily_Metting.Repositories
{

    public class ValueRepository : IValueRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public ValueRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public void AddSubmissionValue(Value value)
        {
            _dailyMeetingDbContext.Values.Add(value);
            _dailyMeetingDbContext.SaveChanges();
        }

        public void DeleteValuesBySubmission(Submission submission)
        {
            var valuesToDelete = _dailyMeetingDbContext.Values.Where(v=>v.Submission == submission);
            _dailyMeetingDbContext.Values.RemoveRange(valuesToDelete);
            _dailyMeetingDbContext.SaveChanges();
        }

        public List<Value> GetSubmissionValue(int submissionID)
        {

            return _dailyMeetingDbContext.Values.Where(s=>s.Submission.SubmissionID == submissionID).ToList();
        }

        public void UpdateSubmissionValues(List<Value> values)
        {
            foreach (var value in values)
            {
                var val = _dailyMeetingDbContext.Values.Find(value.ValueID);
                if (val != null)
                {
                    // Update the values properties
                    val.Value_point = val.Value_point;
                    val.description = val.description;
                    val.comment = val.comment;

                   
                }
                else { 
                    _dailyMeetingDbContext.Values.Add(value); 
                }
                // Save the changes to the database
                _dailyMeetingDbContext.SaveChanges();
            }

        }
    }
}
