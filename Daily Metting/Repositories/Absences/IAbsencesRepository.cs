using Daily_Metting.Models;

namespace Daily_Metting.Repositories.Absences
{
    public interface IAbsencesRepository
    {
        IEnumerable<Absence> AllAbsences { get; }
        void AddAbsence(Absence submission);
        List<Absence> GetAbsences(DateTime date);
        int GetAbcencesCountByStatus_User(string Status, User user);
        void UpdateAbsences(User user, DateTime date, string status);
    }
}
