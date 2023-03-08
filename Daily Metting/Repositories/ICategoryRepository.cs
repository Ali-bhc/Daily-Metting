using Daily_Metting.Models;

namespace Daily_Metting.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> AllCategories { get; }



    }
}
