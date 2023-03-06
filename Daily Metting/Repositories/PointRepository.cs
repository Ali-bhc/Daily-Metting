using Daily_Metting.DAO;
using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public class PointRepository:IPointRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public PointRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public IEnumerable<Point> AllPoints => _dailyMeetingDbContext.Points.OrderBy(p => p.Point_Name);
        
        public Point GetByID(int pointID) {
            return _dailyMeetingDbContext.Points.Where(p => p.PointID == pointID).FirstOrDefault();
        }

        public IEnumerable<Point> GetPointsByDepartement_Category(string Departement,string category_name)
        {
            if (!string.IsNullOrEmpty(category_name))
            {
                switch (Departement)
                {
                    case "WH": return _dailyMeetingDbContext.Points.Where(p => p.WH_Acces == true && p.Category.Category_Name.Equals(category_name)).ToList(); break;
                    case "CC_PP": return _dailyMeetingDbContext.Points.Where(p => p.CS_PP_Acces == true && p.Category.Category_Name.Equals(category_name)).ToList(); break;
                    case "Procurement": return _dailyMeetingDbContext.Points.Where(p => p.Procurement_Acces == true && p.Category.Category_Name.Equals(category_name)).ToList(); break;
                    default: return _dailyMeetingDbContext.Points.Where(p => p.Category.Category_Name.Equals(category_name)).ToList(); ;
                }
            }
            else
            {
                switch (Departement)
                {
                    case "WH": return _dailyMeetingDbContext.Points.Where(p => p.WH_Acces == true).ToList(); break;
                    case "CC_PP": return _dailyMeetingDbContext.Points.Where(p => p.CS_PP_Acces == true).ToList(); break;
                    case "Procurement": return _dailyMeetingDbContext.Points.Where(p => p.Procurement_Acces == true).ToList(); break;
                    default: return AllPoints;
                }
            }

        }

      
    }
}
