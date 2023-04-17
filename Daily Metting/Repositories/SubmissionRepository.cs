using Daily_Metting.DAO;
using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Drawing.Printing;

namespace Daily_Metting.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;
        public SubmissionRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }


        public IEnumerable<Submission> AllSubmission => _dailyMeetingDbContext.Submissions.OrderBy(p=> p.submission_time);


        public void AddSubmission(Submission submission)
        {
            _dailyMeetingDbContext.Submissions.Add(submission);
            _dailyMeetingDbContext.SaveChanges();       
        }

        public void DeleteSubmission(Submission submission)
        {
            _dailyMeetingDbContext.Submissions.Remove(submission);
        }



        public List<Submission> GetAllSubmissionByPage(int page, int pagesize)
        {
            var submissions = _dailyMeetingDbContext.Submissions
                            .OrderByDescending(s => s.submission_time)
                            .Skip((page - 1) * pagesize)
                            .Take(pagesize).Include(s => s.User)
                            .ToList();
            return submissions;
        }

        
        public Submission GetSubmissionByUser_Date(DateTime date, User user)
        {
           return _dailyMeetingDbContext.Submissions.Where(s => EF.Functions.DateDiffDay(s.submission_time, date) == 0 && s.User == user ).Include(s => s.User).FirstOrDefault();
        }

        public List<Submission> GetSubmissionsByDate(DateTime date)
        {
            return _dailyMeetingDbContext.Submissions.Where(s => EF.Functions.DateDiffDay(s.submission_time, date) == 0).ToList();
        }

        public List<Submission> GetUserSubmission(User user)
        {
            return _dailyMeetingDbContext.Submissions.Where(s=>s.User==user).ToList();
        }




        public Submission GetUserSubmissionById(int subId)
        {
            return _dailyMeetingDbContext.Submissions.Where(s=>s.SubmissionID == subId).Include(s => s.User).FirstOrDefault();
        }



        public List<Submission> GetUserSubmissionByPage(int page, int pageSize, User user)
        {
            var submissions = _dailyMeetingDbContext.Submissions
                                .Where(s => s.User == user)
                                .OrderByDescending(s => s.submission_time)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            return submissions;
        }

        public int GetStatusCountByUser(string status, User user)
        {
            return _dailyMeetingDbContext.Submissions.Where(s=>s.User==user && s.status==status).Count();
        }

        public List<Submission> GetYesterdaySubmissions()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var submissions = _dailyMeetingDbContext.Submissions
                .Where(s => s.submission_time < DateTime.Today && s.submission_time >= yesterday).Include(s=>s.User)
                .ToList();
            return submissions;
        }
       
        public List<Submission> GetTodaySubmissions()
        {
            var today = DateTime.Today.Date;
            var submissions = _dailyMeetingDbContext.Submissions
                .Where(s => s.submission_time < DateTime.Now && s.submission_time >= today).Include(s => s.User)
                .ToList();
            return submissions;
        }
    }
}
