namespace Daily_Metting.Models
{
    public class APU
    {
        public int Id { get; set; }
        public string APU_Name { get; set; }
        public double Attainement_min { get; set; } = 0;
        public double Attainement_Max { get; set; }
        public List<Attainement> Attainement { get;set; }

    }
}
