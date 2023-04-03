using Daily_Metting.DAO;
using Daily_Metting.Models;
using Microsoft.EntityFrameworkCore;

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
            return _dailyMeetingDbContext.Users.Where(p => p.UserName == username).FirstOrDefault();
        }

        public List<User> GetMembers()
        {
            return _dailyMeetingDbContext.Users.Where(u => u.IsAdmin == false).ToList();
        }

        public int GetDepartementUsersCount(string dep)
        {
            return _dailyMeetingDbContext.Users.Where(u => u.Departement == dep).Count();
        }

        public void updateUserMissedsubmission(Dictionary<string, List<Submission>> users_submissions)
        {
            foreach (var user in this.GetMembers())
            {
                if (!users_submissions.ContainsKey(user.Id))
                {
                    user.MissedSubmissions = user.MissedSubmissions + 1;
                }
            }

            _dailyMeetingDbContext.SaveChangesAsync();
        }

        public int GetUsersMissedSubmissions(User user)
        {
            return _dailyMeetingDbContext.Users.Where(u => u.Id == user.Id).Select(u => u.MissedSubmissions).FirstOrDefault();
        }
    }
}
