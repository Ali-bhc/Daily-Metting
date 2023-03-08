using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IAbsencesRepository
    {
        IEnumerable<Absence> AllAbsences { get; }
        void AddAbsence(Absence submission);
        Absence GetAbsence(DateTime date);
    }
}
