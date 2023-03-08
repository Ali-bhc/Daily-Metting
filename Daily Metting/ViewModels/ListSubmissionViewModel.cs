using Daily_Metting.Models;

namespace Daily_Metting.ViewModels
{
    public class ListSubmissionViewModel
    {
        public List<Submission> Submissions { get;}
        public int total { get; set;}
        public int page { get; set;}
        public int pageSize { get; set;}
        public int totalPages { get; set;}

        public ListSubmissionViewModel(List<Submission> submissions, int total, int page, int pageSize) : this(submissions)
        {
            this.total = total;
            this.page = page;
            this.pageSize = pageSize;
            this.totalPages = total/pageSize;
        }

        public ListSubmissionViewModel(List<Submission> submissions)
        {
            Submissions = submissions;
        }
    }
}
