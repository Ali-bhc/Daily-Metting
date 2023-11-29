using Daily_Metting.Data;
using Daily_Metting.Models;

namespace Daily_Metting.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DailyMeetingDbContext _dailyMeetingDbContext;

        public CategoryRepository(DailyMeetingDbContext dailyMeetingDbContext)
        {
            _dailyMeetingDbContext = dailyMeetingDbContext;
        }

        public IEnumerable<Category> AllCategories => _dailyMeetingDbContext.Categories.OrderBy(p => p.Category_Name);


    }
}
