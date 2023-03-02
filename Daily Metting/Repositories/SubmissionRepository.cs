using Daily_Metting.DAO;
using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace Daily_Metting.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public IEnumerable<Submission> AllSubmission => _dailyMeetingDbContext.Submissions.OrderBy(p=> p.submission_time);

        public void AddSubmission(Submission submission,List<Value> values)
        {

            foreach (var valueModel in values)
            {
                var value = new Value
                {
                    Value_point = valueModel.Value_point,
                    description = valueModel.description,
                    comment = valueModel.comment,
                    Point = valueModel.Point,
                    Submission = submission
                };

                submission.Values.Add(value);
            }
            //List<ShoppingCartItem>? shoppingCartItems = _shoppingCart.ShoppingCartItems;
            //order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            //order.OrderDetails = new List<OrderDetail>();

            _dailyMeetingDbContext.Submissions.Add(submission);
            _dailyMeetingDbContext.SaveChanges();
            
        }
    }
}
