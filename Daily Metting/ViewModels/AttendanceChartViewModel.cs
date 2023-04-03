namespace Daily_Metting.ViewModels
{
    public class AttendanceChartViewModel
    {
        public string Username { get; set; }
        public int PresentCount { get; set; }
        public int LateCount { get; set; }
        public int AbsentCount { get; set; }
        public int DelegatedCount { get; set; }
    }
}
