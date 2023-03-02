namespace Daily_Metting.Models
{
    public class Value
    {
        public int ValueID { get; set; }
        public string Value_point { get; set; }
        public string description { get; set; }
        public string comment{ get; set; }  
        public Point Point  { get; set; }
        public Submission Submission{ get; set; }

    }
}
