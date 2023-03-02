namespace Daily_Metting.Models
{
    public class Point
    {
        public int PointID { get; set; }
        public string Point_Name { get; set; }
        public bool WH_Acces { get; set; }
        public bool CS_PP_Acces { get; set; }
        public bool Procurement_Acces { get; set; }
        public bool HasMultipleValues { get; set; }
        public Category Category{ get; set;}
        public List<Value> Values { get; set; }

    }
}
