using Daily_Metting.DAO;
using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            return _dailyMeetingDbContext.Users.Where(u => u.IsAdmin == false && u.IsActive == true).ToList();
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

        public List<User> GetTeamMembers()
        {
            return _dailyMeetingDbContext.Users.Where(u => u.IsAdmin == false).ToList();
        }

        public async void UpdateUser(UpdateUserViewModel updateUserViewModel)
        {
            var user = _dailyMeetingDbContext.Users.Where(u => u.Id == updateUserViewModel.Id).FirstOrDefault();
            if (user != null)
            {
                user.Name = updateUserViewModel.Name;
                user.UserName = updateUserViewModel.Username;
                user.Departement = updateUserViewModel.Departement;
                //user.Email = updateUserViewModel.Email;
                user.IsActive = updateUserViewModel.IsActive;
            }
            _dailyMeetingDbContext.Users.Update(user);
            _dailyMeetingDbContext.SaveChanges();
        }
    }
}
