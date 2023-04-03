using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class AttainementsViewModel
    {
        public string Project_name { get; set; }
        public double Attainement_OTIF { get; set; }
        public double Attainement_Mix { get; set; }
        public double Productivity { get; set; }
        public double Downtime { get; set; }
        public double Scrap { get; set; }
        public string? Comment { get; set; }
        public string ApuName { get; set; }
        //public List<APU>? Apus{ get; set; }
    }
}
