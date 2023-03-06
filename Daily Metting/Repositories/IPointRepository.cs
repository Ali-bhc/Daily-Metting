using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface IPointRepository
    {
        IEnumerable<Point> AllPoints { get; }
        IEnumerable<Point> GetPointsByDepartement_Category(string Departement, string category_name);
        public Point GetByID(int pointID);

        //IEnumerable<Point> GetPointsByCategory(string Departement,string category_name);



    }
}
