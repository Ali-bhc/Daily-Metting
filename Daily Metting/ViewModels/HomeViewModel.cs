using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class HomeViewModel
    {
        public Dictionary<string, List<Point>> pointCategoryList { get; set; }
        //public Dictionary<string, List<string>> StringsvaluesPoint { get; set; }
        //public List<KeyValuePair<string, int>> ListOfSumOfValuesOfPoints { get; set; }
        public Dictionary<string,int> ListOfSumOfValuesOfPoints { get; set; }
        public Dictionary<string, List<string>> CommentsConcatenation { get; set; }
        public Dictionary<string, List<string>> DescriptionssConcatenation { get; set; }
        public Dictionary<string, List<Attainement>> ListofAttainementAverages { get;set;}
        public Dictionary<string, string> AugmentationStatus { get; set; }
        //public string Escalation { get; set; }
        //public string comments { get; set; }

        public HomeViewModel(Dictionary<string, List<Point>> pointCategoryList, Dictionary<string, int> listOfSumOfValuesOfPoints, 
            Dictionary<string, List<string>> commentsConcatenation, Dictionary<string, List<string>> descriptionssConcatenation, 
            Dictionary<string, List<Attainement>> listofAttainementAverages, Dictionary<string, string>  augmentationStatus)
        {
            this.pointCategoryList = pointCategoryList;
            ListOfSumOfValuesOfPoints = listOfSumOfValuesOfPoints;
            CommentsConcatenation = commentsConcatenation;
            DescriptionssConcatenation = descriptionssConcatenation;
            ListofAttainementAverages = listofAttainementAverages;
            AugmentationStatus = augmentationStatus;
        }

        public HomeViewModel(Dictionary<string, List<Point>> pointCategoryList, Dictionary<string, int> listOfSumOfValuesOfPoints, 
            Dictionary<string, List<string>> commentsConcatenation, Dictionary<string, List<string>> descriptionssConcatenation,
            Dictionary<string, string> augmentationStatus)
        {
            this.pointCategoryList = pointCategoryList;
            ListOfSumOfValuesOfPoints = listOfSumOfValuesOfPoints;
            CommentsConcatenation = commentsConcatenation;
            DescriptionssConcatenation = descriptionssConcatenation;
            AugmentationStatus = augmentationStatus;
        }

        public HomeViewModel()
        {
        }


        //public HomeViewModel(Dictionary<string, List<Point>> pointCategoryList, 
        //    List<KeyValuePair<string, int>> listOfSumOfValuesOfPoints, 
        //    Dictionary<string, List<string>> commentsConcatenation, 
        //    Dictionary<string, List<string>> descriptionsConcatenation,
        //    Dictionary<string, List<Attainement>> listofAttainementAverages)
        //{
        //    this.pointCategoryList = pointCategoryList;
        //    //StringsvaluesPoint = stringsvaluesPoint;
        //    ListOfSumOfValuesOfPoints = listOfSumOfValuesOfPoints;
        //    CommentsConcatenation = commentsConcatenation;
        //    DescriptionssConcatenation= descriptionsConcatenation;
        //    ListofAttainementAverages= listofAttainementAverages;
        //}


        //public HomeViewModel(Dictionary<string, List<Point>> pointCategoryList, List<KeyValuePair<string, int>> listOfSumOfValuesOfPoints, Dictionary<string, List<string>> commentsConcatenation, Dictionary<string, List<string>> descriptionssConcatenation)
        //{
        //    this.pointCategoryList = pointCategoryList;
        //    ListOfSumOfValuesOfPoints = listOfSumOfValuesOfPoints;
        //    CommentsConcatenation = commentsConcatenation;
        //    DescriptionssConcatenation = descriptionssConcatenation;
        //}


    }
}
