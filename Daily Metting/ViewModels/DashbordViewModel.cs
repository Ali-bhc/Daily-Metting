namespace Daily_Metting.ViewModels
{
    public class DashbordViewModel
    {
        public List<SubmissionsStatusViewModel> UsersSubmissionsStatus { get; set;}
        public List<AttendanceChartViewModel> AttendanceChartViewModels{ get; set;}
        public Dictionary<string,int> UsersDepartementCount { get; set;}

        public DashbordViewModel(List<SubmissionsStatusViewModel> usersSubmissionsStatus, List<AttendanceChartViewModel> attendanceChartViewModels, Dictionary<string, int> usersDepartementCount)
        {
            UsersSubmissionsStatus = usersSubmissionsStatus;
            AttendanceChartViewModels = attendanceChartViewModels;
            UsersDepartementCount = usersDepartementCount;
        }
    }
}
