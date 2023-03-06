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
        private readonly IValueRepository _valueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPointRepository _pointRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public MemberController(IValueRepository valueRepositoryy,ICategoryRepository categoryRepository, IPointRepository pointRepository , ISubmissionRepository submissionRepository ,SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _valueRepository = valueRepositoryy;
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
            Dictionary<string, IEnumerable<Point> > PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            categories = _categoryRepository.AllCategories;
            foreach (var category in categories)
            {
                PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
            }
            ViewBag.MyData = PointCategoryList;
            //List<Value> values= new List<Value>() { new Value { Value_point="12", description="aaaaaa",comment="Agrren", Point= PointCategoryList.FirstOrDefault().Value.FirstOrDefault() } };
            SubmissionViewModel _submissionViewModel = new SubmissionViewModel();


            return View(_submissionViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddSubmission(SubmissionViewModel _submissionViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Point> points;
            IEnumerable<Category> categories;
            Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            categories = _categoryRepository.AllCategories;
            foreach (var category in categories)
            {
                PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
            }
            ViewBag.MyData = PointCategoryList;

            //Step1:Add Submission First
            var submission = new Submission
            {
                User = user, // Set the user property here
                submission_time = DateTime.Now, // Set the date property here
            };
            _submissionRepository.AddSubmission(submission);

            //Step2:A
            foreach (var val in _submissionViewModel.Values)
            {
                var pt = _pointRepository.GetByID(val.PointID);
                _valueRepository.AddSubmissionValue(new Value { Value_point = val.Value_point, description = val.description, comment = val.comment, Point = pt ,Submission=submission});
            }

           // return RedirectToAction(nameof(MemberController.Index), "Member");
           return View(_submissionViewModel);
        }


    }
}
