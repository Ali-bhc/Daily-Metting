using Daily_Metting.Models;
using Daily_Metting.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "MemberPolicy")]
    public class MemberController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPointRepository _pointRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public MemberController(ICategoryRepository categoryRepository, IPointRepository pointRepository, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _pointRepository = pointRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> AddSubmission() 
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Point> points;
            IEnumerable<Category> categories;
            IEnumerable<Point> Safety_points;
            Dictionary<string, IEnumerable<Point> > PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            categories = _categoryRepository.AllCategories;
            foreach (var category in categories)
            {
                PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
            }
            //points = _pointRepository.AllPoints;
            //points = _pointRepository.GetPointsByCategory(category_name);

            
            return View(PointCategoryList);
        }
    }
}
