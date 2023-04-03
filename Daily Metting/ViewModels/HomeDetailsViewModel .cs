using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class HomeDetailsViewModel
    {
        public Dictionary<string, List<Point>> PointCategoryList { get; set; }
        public Dictionary<string, List<Value>> ValuesPoint { get; set; }
        public Dictionary<string, List<Attainement>> ListofAttainementAverages { get; set; }


        public HomeDetailsViewModel(Dictionary<string, List<Point>> pointCategoryList, Dictionary<string, List<Value>> valuesPoint, Dictionary<string, List<Attainement>> listofAttainementAverages)
        {
            PointCategoryList = pointCategoryList;
            ValuesPoint = valuesPoint;
            ListofAttainementAverages = listofAttainementAverages;
        }

        public HomeDetailsViewModel(Dictionary<string, List<Point>> pointCategoryList, Dictionary<string, List<Value>> valuesPoint)
        {
            PointCategoryList = pointCategoryList;
            ValuesPoint = valuesPoint;
        }
    }
}
