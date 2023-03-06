using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IUserRepository
    {

        IEnumerable<User> AllUsers { get; }
        User? GetByUsername(string username);

    }
}
