using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class SubmissionDetailsViewModel
    {
        public Dictionary<string, IEnumerable<Point>> PointCategoryList { get; set; }
        public Dictionary<string,Value> PointValues { get; set; }
        public Dictionary<string, List<Attainement>> ListofAttainement { get; set; }


        public SubmissionDetailsViewModel() { }

        public SubmissionDetailsViewModel(Dictionary<string, Value> pointValues, Dictionary<string, IEnumerable<Point>> pointCategoryList)
        {
            this.PointValues = pointValues;
            PointCategoryList = pointCategoryList;
        }
    }
}
