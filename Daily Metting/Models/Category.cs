using System.Collections;

namespace Daily_Metting.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public int Category_Name { get; set;}
        public ICollection<Point> Points { get; set; }

        public Category(int categoryID, int category_Name, ICollection<Point> points)
        {
            CategoryID = categoryID;
            Category_Name = category_Name;
            Points = points;
        }
    }
}
