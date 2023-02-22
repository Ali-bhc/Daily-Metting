namespace Daily_Metting.Models
{
    public class Point
    {
        public int PointID { get; set; }
        public string Point_Name { get; set; }
        public Category Category{ get; set;}
        public ICollection<Value> Values { get; set; }

        public Point(int pointID, string point_Name, Category category, ICollection<Value> values)
        {
            PointID = pointID;
            Point_Name = point_Name;
            Category = category;
            Values = values;
        }
    }
}
