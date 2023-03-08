using Daily_Metting.DAO;
using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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

        public Submission GetSubmissionByUser_Date(DateTime date, User user)
        {
           return _dailyMeetingDbContext.Submissions.Where(s => s.User == user && s.submission_time==date).FirstOrDefault();
        }

        public List<Submission> GetUserSubmission(User user)
        {
            return _dailyMeetingDbContext.Submissions.Where(s=>s.User==user).ToList();
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

    }
    }
