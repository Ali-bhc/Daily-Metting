using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class SubmissionViewModel
    {
       // public List<ValueViewModel>? UpdateValueslist { get; set; }
        public Dictionary <string, ValueViewModel>? Values { get; set; }
        public Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
        public bool IsSubmited { get; set; }
        public bool IsMissed { get; set; }=false;
        public bool Is_CS_PP { get; set; }
        //public IFormFile file { get; set; }
        //public List<Attainement> Attainements { get; set; }
        public Dictionary<string, List<string>> ProjectList { get; set; }
        public Dictionary<string, AttainementsViewModel> AttainamentsList { get; set; }


        public SubmissionViewModel()
        {
        }

       

        public SubmissionViewModel(bool isSubmited, bool isMissed)
        {
            IsSubmited = isSubmited;
            IsMissed = isMissed;
        }

        public SubmissionViewModel(Dictionary<string, IEnumerable<Point>> pointCategoryList, bool isSubmited, bool is_CS_PP) 
        {
            PointCategoryList = pointCategoryList;
            IsSubmited = isSubmited;
            Is_CS_PP = is_CS_PP;
        }

        public SubmissionViewModel(Dictionary<string, IEnumerable<Point>> pointCategoryList, Dictionary<string, List<string>> projectList , bool isSubmited , bool is_CS_PP)
        {
            this.PointCategoryList = pointCategoryList;
            this.ProjectList = projectList;
            this.IsSubmited= isSubmited;
            this.Is_CS_PP= is_CS_PP;
        }


        public SubmissionViewModel(Dictionary<string, ValueViewModel>? values, Dictionary<string, IEnumerable<Point>> pointCategoryList, Dictionary<string, AttainementsViewModel> attainamentsList,Dictionary<string,List<string>> projectList , bool is_CS_PP)
        {
            this.Values = values;
            AttainamentsList = attainamentsList;
            this.PointCategoryList = pointCategoryList;
            ProjectList = projectList;
            Is_CS_PP = is_CS_PP;
        }

        public SubmissionViewModel(Dictionary<string, ValueViewModel>? values, Dictionary<string, IEnumerable<Point>> pointCategoryList, bool is_CS_PP)
        {
            Values = values;
            PointCategoryList = pointCategoryList;
            Is_CS_PP = is_CS_PP;
        }
    }
}
