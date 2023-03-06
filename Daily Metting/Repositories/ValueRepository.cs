using Daily_Metting.DAO;
using Daily_Metting.Models;

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
    }
}
