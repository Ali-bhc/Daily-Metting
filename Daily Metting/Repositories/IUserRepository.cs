using Daily_Metting.Models;
using Daily_Metting.ViewModels;

namespace Daily_Metting.Repositories
{
    public interface IUserRepository
    {

        IEnumerable<User> AllUsers { get; }
        User? GetByUsername(string username);
        List<User> GetMembers();
        List<User> GetTeamMembers();
        int GetDepartementUsersCount(string dep);
        void updateUserMissedsubmission(Dictionary<string, List<Submission>> users_submissions);
        int GetUsersMissedSubmissions(User user);
        void UpdateUser(UpdateUserViewModel updateUserViewModel);

    }
}
