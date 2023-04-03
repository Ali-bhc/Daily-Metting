namespace Daily_Metting.Models
{
    public class Attainement
    {
        public int Id { get; set; }
        public String Project_name { get; set; }
        public double Attainement_OTIF { get; set;}
        public double Attainement_Mix { get; set;}
        public double Productivity { get; set;}
        public double Downtime { get; set;}
        public double Scrap { get; set;}
        public string ?Comment { get; set;}
        public APU APU { get; set;}
        public Submission Submission { get; set;}

    }
}
