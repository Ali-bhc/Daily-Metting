using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class AbsenceViewModel
    {
        public string status { get; set; }
        public string username { get; set; }

        public AbsenceViewModel(string status, string user)
        {
            this.status = status;
            this.username = user;
        }

        public AbsenceViewModel()
        {
        }
    }
}
