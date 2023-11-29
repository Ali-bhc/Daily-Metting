using Daily_Metting.Models;

namespace Daily_Metting.Repositories.Categories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> AllCategories { get; }



    }
}
