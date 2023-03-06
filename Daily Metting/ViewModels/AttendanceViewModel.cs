namespace Daily_Metting.ViewModels
{
    public class AttendanceViewModel
    {
        public List<AbsenceViewModel>? AttendanceStatus { get; set; }

        public AttendanceViewModel(List<AbsenceViewModel>? attendanceStatus)
        {
            AttendanceStatus = attendanceStatus;
        }

        public AttendanceViewModel()
        {
        }
    }
}
