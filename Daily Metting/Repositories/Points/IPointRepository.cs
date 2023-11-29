using Daily_Metting.Models;

namespace Daily_Metting.Repositories.Points
{
    public interface IPointRepository
    {
        IEnumerable<Point> AllPoints { get; }
        IEnumerable<Point> GetPointsByDepartement_Category(string Departement, string category_name);
        public Point GetByID(int pointID);
        public Point GetByName(string pointName);

        List<Point> GetPointsByCategory(int category_id);
        List<Point> GetPointsByDepartement(string dep);



    }
}
