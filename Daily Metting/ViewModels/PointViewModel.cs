using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class PointViewModel
    {
        public string Point_Name { get; set; }
        public bool WH_Acces { get; set; }
        public bool CS_PP_Acces { get; set; }
        public bool Procurement_Acces { get; set; }
        public bool HasMultipleValues { get; set; }
        public int CategoryID { get; set; }
        public List<Category>? categories { get; set;}
    }
}
