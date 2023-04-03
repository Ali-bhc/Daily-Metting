using Daily_Metting.DAO;
using Daily_Metting.Models;
using Microsoft.EntityFrameworkCore;

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

        
        public List<Absence> GetAbsences(DateTime date)
        {
            return _dailyMeetingDbContext.Absences.Where(a => EF.Functions.DateDiffDay(a.date, date) == 0).Include(a => a.User).ToList();
            
        }

        public int GetAbcencesCountByStatus_User(string status, User user)
        {
            return _dailyMeetingDbContext.Absences.Where(a => a.Status == status && a.User==user).Count();
        }


        public void UpdateAbsences(User user, DateTime date, string abs_status)
        {
            var abs = _dailyMeetingDbContext.Absences.Where(a=>a.User==user && EF.Functions.DateDiffDay(a.date, date) == 0).FirstOrDefault();
            if (abs != null)
            {
                if (abs.Status != abs_status)
                {
                    abs.Status = abs_status;
                    _dailyMeetingDbContext.SaveChanges();
                }
            }
        }
    }
}
