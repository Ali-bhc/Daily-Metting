using System.Collections;

namespace Daily_Metting.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public String Category_Name { get; set;}
        public List<Point> Points { get; set; }

    }
}
