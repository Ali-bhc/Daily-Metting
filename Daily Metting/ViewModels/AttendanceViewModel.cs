using Daily_Metting.Models;
namespace Daily_Metting.ViewModels
{
    public class AttendanceViewModel
    {
        public List<AbsenceViewModel>? AttendanceStatus { get; set; }
        public List<User>? Users { get; set; }
        public bool IsActive { get; set; }

        public AttendanceViewModel(List<AbsenceViewModel>? attendanceStatus)
        {
            AttendanceStatus = attendanceStatus;
        }

        public AttendanceViewModel()
        {
        }

        public AttendanceViewModel(List<User>? users)
        {
            Users = users;
        }

        public AttendanceViewModel(List<User>? users, bool isActive) : this(users)
        {
            IsActive = isActive;
        }

        public AttendanceViewModel(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
