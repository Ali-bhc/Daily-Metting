using Daily_Metting.Data;
using Daily_Metting.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using System;

namespace Daily_Metting.Repositories.Attainements
{
    public class AttaienementRepository : IAttainementRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;
        public AttaienementRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public List<Attainement> AllAttainement => _dailyMeetingDbContext.Attainements.OrderBy(p => p.APU).ToList();

        public void AddAttainement(Attainement attainement)
        {
            _dailyMeetingDbContext.Attainements.Add(attainement);
            _dailyMeetingDbContext.SaveChanges();
        }

        public void DeleteAttainementsBySubmission(Submission submission)
        {
            var AttainementsToDelete = _dailyMeetingDbContext.Attainements.Where(v => v.Submission == submission);
            _dailyMeetingDbContext.Attainements.RemoveRange(AttainementsToDelete);
            _dailyMeetingDbContext.SaveChanges();
        }


        public List<Attainement> GetAttainementsBySubmission(Submission submission)
        {
            var AttainementsList = _dailyMeetingDbContext.Attainements.Where(a => a.Submission == submission).ToList();
            foreach (var att in AttainementsList)
            {
                att.Attainement_OTIF = Convert.ToDouble((att.Attainement_OTIF * 100).ToString("0.00"));
                att.Attainement_Mix = Convert.ToDouble((att.Attainement_Mix * 100).ToString("0.00"));
                att.Productivity = Convert.ToDouble((att.Productivity * 100).ToString("0.00"));
                att.Downtime = Convert.ToDouble((att.Downtime * 100).ToString("0.00"));
                att.Scrap = Convert.ToDouble((att.Scrap * 100).ToString("0.00"));
            }
            return AttainementsList;
        }

        public List<string> GetProjectList(APU aPU, DateTime date)
        {
            var AttainementList = _dailyMeetingDbContext.Attainements.Where(a => a.APU == aPU && EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0).Select(a => a.Project_name).Distinct().ToList();
            if (AttainementList == null)
            {
                AttainementList = new List<string> { "Victor Battery", "PT Battery", "Drive unit", "HMI", "DUPTaped", "DUP Sheated", "Cobs", "Minus / plus", "Victor Bike", "SCM", "PT Chargingsocket", "PT Splitter", "Speed", "Light" };
            }
            AttainementList.Reverse();
            return AttainementList;
        }

        public Attainement GetAttainementsAverage(string project_name, DateTime date)
        {
            //Console.WriteLine(project_name);
            var attainement_otif_average = _dailyMeetingDbContext.Attainements.
                Where(a => EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0 && a.Project_name == project_name)
                .Select(a => a.Attainement_OTIF).Average();
            var attainement_mix_average = _dailyMeetingDbContext.Attainements.
                Where(a => EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0 && a.Project_name == project_name)
                .Select(a => a.Attainement_Mix).Average();
            var productivity_average = _dailyMeetingDbContext.Attainements.
                Where(a => EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0 && a.Project_name == project_name)
                .Select(a => a.Productivity).Average();
            var downtime_average = _dailyMeetingDbContext.Attainements.
                Where(a => EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0 && a.Project_name == project_name)
                .Select(a => a.Downtime).Average();
            var scrap_average = _dailyMeetingDbContext.Attainements.
                Where(a => EF.Functions.DateDiffDay(a.Submission.submission_time, date) == 0 && a.Project_name == project_name)
                .Select(a => a.Scrap).Average();

            var comments = _dailyMeetingDbContext.Attainements
                .Where(a => a.Project_name == project_name && a.Submission.submission_time.Date == date)
                .Select(v => v.Comment).ToList();
            string Comment = string.Join(Environment.NewLine, comments);

            var attaienemnt = new Attainement
            {
                Project_name = project_name,
                Attainement_OTIF = Convert.ToDouble((attainement_otif_average * 100).ToString("0.00")),
                Attainement_Mix = Convert.ToDouble((attainement_mix_average * 100).ToString("0.00")),
                Productivity = Convert.ToDouble((productivity_average * 100).ToString("0.00")),
                Downtime = Convert.ToDouble((downtime_average * 100).ToString("0.00")),
                Scrap = Convert.ToDouble((scrap_average * 100).ToString("0.00")),
                Comment = Comment
            };
            return attaienemnt;

        }

        public bool isAttainementsExist(DateTime date)
        {
            var attainements = _dailyMeetingDbContext.Attainements.Where(s => EF.Functions.DateDiffDay(s.Submission.submission_time, date) == 0);
            if (attainements.Any())
            {
                return true;
            }
            else { return false; }

        }

        public List<Attainement> GetAttainementBySubmission(int submissionId)
        {
            return _dailyMeetingDbContext.Attainements.Where(a => a.Submission.SubmissionID == submissionId).ToList();
        }

        public List<Attainement> GetAttainementsBySubmission_Apu(Submission submission, APU aPU)
        {
            var AttainemenList = _dailyMeetingDbContext.Attainements.Where(a => a.Submission == submission && a.APU == aPU).ToList();
            foreach (var att in AttainemenList)
            {
                att.Attainement_OTIF = Convert.ToDouble((att.Attainement_OTIF * 100).ToString("0.00"));
                att.Attainement_Mix = Convert.ToDouble((att.Attainement_Mix * 100).ToString("0.00"));
                att.Productivity = Convert.ToDouble((att.Attainement_Mix * 100).ToString("0.00"));
                att.Downtime = Convert.ToDouble((att.Attainement_Mix * 100).ToString("0.00"));
                att.Scrap = Convert.ToDouble((att.Attainement_Mix * 100).ToString("0.00"));
            }
            return _dailyMeetingDbContext.Attainements.Where(a => a.Submission == submission && a.APU == aPU).ToList();
        }
    }
}
