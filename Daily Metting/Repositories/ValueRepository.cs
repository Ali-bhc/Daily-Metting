using Daily_Metting.DAO;
using Daily_Metting.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace Daily_Metting.Repositories
{

    public class ValueRepository : IValueRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public ValueRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public void AddSubmissionValue(Value value)
        {
            _dailyMeetingDbContext.Values.Add(value);
            _dailyMeetingDbContext.SaveChanges();
        }

        public void DeleteValuesBySubmission(Submission submission)
        {
            var valuesToDelete = _dailyMeetingDbContext.Values.Where(v=>v.Submission == submission);
            _dailyMeetingDbContext.Values.RemoveRange(valuesToDelete);
            _dailyMeetingDbContext.SaveChanges();
        }

        public List<Value> GetSubmissionValue(int submissionID)
        {

            return _dailyMeetingDbContext.Values.Where(s=>s.Submission.SubmissionID == submissionID).ToList();
        }

        public Value GetValueBySubmissionPoint(int submissionId, Point point)
        {
            Value value = _dailyMeetingDbContext.Values.Where(v=>v.Submission.SubmissionID==submissionId && v.Point == point).FirstOrDefault();
            return value;
        }

        public void UpdateSubmissionValues(List<Value> values)
        {
            foreach (var value in values)
            {
                var val = _dailyMeetingDbContext.Values.Find(value.ValueID);
                if (val != null)
                {
                    // Update the values properties
                    val.Value_point = val.Value_point;
                    val.description = val.description;
                    val.comment = val.comment;

                   
                }
                else { 
                    _dailyMeetingDbContext.Values.Add(value); 
                }
                // Save the changes to the database
                _dailyMeetingDbContext.SaveChanges();
            }

        }

        public List<Value> GetValuesByPoint_SubmissionDate(int pointId , DateTime submission_time)
        {
            var values = _dailyMeetingDbContext.Values.Where(v => v.Point.PointID == pointId && v.Submission.submission_time.Date == submission_time)
                .Include(v=>v.Submission.User).OrderBy(v=>v.Submission.User).ToList();
            return values;
        }

        //public List<String> GetStringsValuesByPoint_SubmissionDate(int pointId, DateTime submission_time)
        //{
        //    var values = _dailyMeetingDbContext.Values.Where(v => v.Point.PointID == pointId && v.Submission.submission_time.Date == submission_time)
        //        .Include(v => v.Submission).OrderBy(v => v.Submission.User).Select(v => v.Value_point).Distinct().ToList();
        //    return values;
        //}


        public int GetSumOfValuesByPoints_SubmissionDate(int pointId, DateTime submission_time) 
        {
            int sumOfValues = 0;
            var valuesList = (int)_dailyMeetingDbContext.Values
                .Where(v => v.Point.PointID == pointId && EF.Functions.DateDiffDay(v.Submission.submission_time, submission_time) == 0)
                .Include(v => v.Submission).OrderBy(v => v.Submission.User).Select(v => v.Value_point).Sum();
            //foreach (var val in valuesList)
            //{
            //    sumOfValues += (int)val;
            //}

            return valuesList;

        }

        public List<string> GetCommentsConcatenations(Point point, DateTime submission_time)
        {
            List<string> commentspointList=new List<string>();
            
                var PointsComments = _dailyMeetingDbContext.Values
                    .Where(v => v.Point.PointID == point.PointID && v.Submission.submission_time.Date == submission_time)
                    .Select(v => v.comment).ToList();
                string Comments = string.Join(Environment.NewLine, PointsComments);
                commentspointList.Add(Comments);

            return commentspointList;
        }

        public List<string> GetDescriptionsConcatenations(Point point, DateTime submission_time)
        {
            List<string> descriptionspointList = new List<string>();

            var PointsDescriptions = _dailyMeetingDbContext.Values
                .Where(v => v.Point.PointID == point.PointID && v.Submission.submission_time.Date == submission_time)
                .Select(v => v.description).ToList();
            string Comments = string.Join(Environment.NewLine, PointsDescriptions);
            descriptionspointList.Add(Comments);

            return descriptionspointList;
        }


        //public List<string> GetMultiValuesCommentsConcatenations(Point point, DateTime submission_time,List<string> values)
        //{
        //    List<string> commentspointList = new List<string>();

        //    foreach (var pt in values)  
        //    {
        //        var PointsComments = _dailyMeetingDbContext.Values
        //            .Where(v => v.Point.PointID == point.PointID && v.Submission.submission_time.Date == submission_time)
        //            .Select(v => v.comment).ToList();
        //        string Comments = string.Join(Environment.NewLine, PointsComments);
        //        commentspointList.Add(Comments);
        //    }
        //    return commentspointList;
        //}



    }
}
