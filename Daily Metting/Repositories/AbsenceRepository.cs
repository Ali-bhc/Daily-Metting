using Daily_Metting.DAO;
using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public class AbsenceRepository:IAbsencesRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;
        public AbsenceRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public IEnumerable<Absence> AllAbsences => _dailyMeetingDbContext.Absences.OrderBy(p => p.date);

        public void AddAbsence(Absence absence)
        {
            _dailyMeetingDbContext.Absences.Add(absence);
            _dailyMeetingDbContext.SaveChanges();
        }
        //public void AddDayAbsence(List<Absence> absences)
        //{
        //    foreach (Absence absence in absences)
        //    {
        //        _dailyMeetingDbContext.Absences.Add(absence);
        //    }
            
        //    _dailyMeetingDbContext.SaveChanges();
        //}
    }
}
