using Daily_Metting.Data;
using Daily_Metting.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace Daily_Metting.Repositories.Apus
{
    public class APURepository : IAPURepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;
        public APURepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public List<APU> AllApu => _dailyMeetingDbContext.APUs.OrderBy(p => p.APU_Name).ToList();

        public void AddAPU(APU aPU)
        {
            _dailyMeetingDbContext.APUs.Add(aPU);
        }

        public APU GetAPUByName(string name)
        {
            return _dailyMeetingDbContext.APUs.Where(a => a.APU_Name == name).FirstOrDefault();
        }

        public void UpdateAPU(int APU_ID, double Attainement_min, double Attainement_Max)
        {
            var aPU = _dailyMeetingDbContext.APUs.Find(APU_ID);
            if (aPU != null)
            {
                if (aPU.Attainement_min != Attainement_min || aPU.Attainement_Max != Attainement_Max)
                {
                    aPU.Attainement_min = Attainement_min;
                    aPU.Attainement_Max = Attainement_Max;
                    _dailyMeetingDbContext.SaveChanges();
                }
            }
        }
    }
}
