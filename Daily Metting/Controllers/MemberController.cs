using Daily_Metting.Models;
using Daily_Metting.Repositories;
using Daily_Metting.ViewModels;
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
        private readonly ISubmissionRepository _submissionRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public MemberController(ICategoryRepository categoryRepository, IPointRepository pointRepository , ISubmissionRepository submissionRepository ,SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _categoryRepository = categoryRepository;
            _pointRepository = pointRepository;
            _submissionRepository=submissionRepository;
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
            ViewData["pointsofcategories"] = PointCategoryList;


            return View(PointCategoryList);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubmission(SubmissionViewModel _submissionViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var submission = new Submission
            {
                User = user, // Set the user property here
                submission_time = DateTime.Now, // Set the date property here
                Values = new List<Value>()
            };
   
            _submissionRepository.AddSubmission(submission,_submissionViewModel.Values);

            return RedirectToAction(nameof(MemberController.Index), "Member");
        }


    }
}
