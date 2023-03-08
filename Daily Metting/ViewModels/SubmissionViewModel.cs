using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class SubmissionViewModel
    {
        public List<ValueViewModel>? Values { get; set; }
        public Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
        public int total_number { get; set; }
        public bool IsSubmited { get; set; }

        public SubmissionViewModel(Dictionary<string, IEnumerable<Point>> pointCategoryList)
        {
            PointCategoryList = pointCategoryList;
        }

        public SubmissionViewModel()
        {
        }

        public SubmissionViewModel(List<ValueViewModel>? values, Dictionary<string, IEnumerable<Point>> pointCategoryList)
        {
            Values = values;
            PointCategoryList = pointCategoryList;
        }

        public SubmissionViewModel(Dictionary<string, IEnumerable<Point>> pointCategoryList, int total_number) : this(pointCategoryList)
        {
            this.total_number = total_number;
        }

        public SubmissionViewModel(List<ValueViewModel>? values)
        {
            Values = values;
        }

        public SubmissionViewModel(bool isSubmited)
        {
            IsSubmited = isSubmited;
        }

        public SubmissionViewModel(Dictionary<string, IEnumerable<Point>> pointCategoryList, int total_number, bool isSubmited) : this(pointCategoryList, total_number)
        {
            IsSubmited = isSubmited;
        }

    }
}
