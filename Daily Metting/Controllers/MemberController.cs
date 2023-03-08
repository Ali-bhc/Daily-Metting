using Daily_Metting.Models;
using Daily_Metting.Repositories;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
            int total;
            IEnumerable<Category> categories;
            Dictionary<string, IEnumerable<Point> > PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            // First we should check if the user already submit
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Today, user);
            if (sub == null)
            {
                ViewBag.Message = "Hello "+user.ToString()+" DO your submission for " + DateTime.Today + ":)";
                categories = _categoryRepository.AllCategories.Reverse();
                foreach (var category in categories)
                {
                    PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
                }

                //List<ValueViewModel> values = new List<ValueViewModel>();
                total = PointCategoryList.Values.Sum(list => list.Count());

                SubmissionViewModel _submissionViewModel = new SubmissionViewModel(PointCategoryList, total,false);
                return View(_submissionViewModel);
            }
            else
            {
                ViewBag.Message = "Hello " + user.ToString() + " you have already submit for today! \nGo check it ";
                SubmissionViewModel _submissionViewModel = new SubmissionViewModel(true);
                return View(_submissionViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSubmission(SubmissionViewModel _submissionViewModel)
        {
        //get the user
            var user = await _userManager.GetUserAsync(User);           
        //Step1:Add Submission First
        var submission = new Submission
            {
                User = user, // Set the user property here
                submission_time = DateTime.Today, // Set the date property here
             };

        _submissionRepository.AddSubmission(submission);
        //Step2:Insert Values from the Model to Database
        foreach (var val in _submissionViewModel.Values)
        {
            var pt = _pointRepository.GetByID(val.PointID);
            _valueRepository.AddSubmissionValue(new Value { Value_point = val.Value_point, description = val.description, comment = val.comment, Point = pt, Submission = submission });
        }

        return RedirectToAction(nameof(MemberController.Index), "Member");
        }



        public async Task<IActionResult> UpdateSubmission()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Point> points;
            int total;
            IEnumerable<Category> categories;
            Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            var valueslist = new List<ValueViewModel>();
            // First we should check if the user already submit
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Today, user);
            var values = _valueRepository.GetSubmissionValue(sub.SubmissionID);
            //get the submission value
            if (sub != null)
            {
                foreach(var val in values)
                {
                    valueslist.Add(new ValueViewModel { Value_point = val.Value_point, description = val.description, comment = val.comment });
                }
            }
            categories = _categoryRepository.AllCategories.Reverse();
            foreach (var category in categories)
            {
                PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
            }
            SubmissionViewModel _submissionViewModel = new SubmissionViewModel(valueslist,PointCategoryList);

            return View(_submissionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSubmission(SubmissionViewModel _submissionViewModel)
        {
            //get the user
            var user = await _userManager.GetUserAsync(User);
            //get the submission
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Today, user);
            //get the old values
            var values = _valueRepository.GetSubmissionValue(sub.SubmissionID);
            _valueRepository.DeleteValuesBySubmission(sub);
            _submissionRepository.DeleteSubmission(sub);

            var submission = new Submission
            {
                User = user, // Set the user property here
                submission_time = DateTime.Today, // Set the date property here
            };

            _submissionRepository.AddSubmission(submission);
            //Step2:Insert Values from the Model to Database
            foreach (var val in _submissionViewModel.Values)
            {
                var pt = _pointRepository.GetByID(val.PointID);
                _valueRepository.AddSubmissionValue(new Value { Value_point = val.Value_point, description = val.description, comment = val.comment, Point = pt, Submission = submission });
            }



            return RedirectToAction(nameof(MemberController.Index), "Member");


        }


        
        [HttpGet]
        public async Task<IActionResult> History(int page = 1)
        {
            int pageSize = 10;
            var user = await _userManager.GetUserAsync(User);
            var submissions_page = _submissionRepository.GetUserSubmissionByPage(page,pageSize,user);
            var total_submissions = _submissionRepository.GetUserSubmission(user).Count();

            ListSubmissionViewModel listsubmissions = new ListSubmissionViewModel(submissions_page, total_submissions,page,pageSize) ;
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SubmissionsTable", listsubmissions);
            }
            else
            {
                return View(listsubmissions);
            }
        }

        //public async Task<IActionResult> SubmissionDetails(int id)
        //{
        //    Submission sub = _submissionRepository.GetUserSubmissionsById(id);
        //    var SubmissionsValues = _valueRepository.GetSubmissionValue(id);
        //    sub.Values= SubmissionsValues;
        //    return View(sub);

        //}




    }
}
