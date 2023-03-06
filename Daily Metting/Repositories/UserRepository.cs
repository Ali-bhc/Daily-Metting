using Daily_Metting.DAO;
using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public UserRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }
        public IEnumerable<User> AllUsers => _dailyMeetingDbContext.Users.ToList().OrderBy(p => p.Name);

        public User? GetByUsername(string username)
        {
          return  _dailyMeetingDbContext.Users.Where(p => p.UserName== username).FirstOrDefault();
        }
    }
}
