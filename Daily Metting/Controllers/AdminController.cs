using Daily_Metting.Models;
using Daily_Metting.Repositories;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using iText.Html2pdf;
using Point = Daily_Metting.Models.Point;
using iText.Layout.Borders;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Image;

namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly SignInManager<User>? _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IAbsencesRepository _absencesRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IValueRepository _valueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPointRepository _pointRepository;
        private readonly IAttainementRepository _attainementRepository;
        private readonly IAPURepository _apuRepository;
        private readonly IWebHostEnvironment _env;


        public AdminController(UserManager<User> userManager, IUserRepository userRepository,
            IAbsencesRepository absencesRepository, ISubmissionRepository submissionRepository,
            IValueRepository valueRepository, ICategoryRepository categoryRepository,
            IPointRepository pointRepository, IAttainementRepository attainementRepository,IAPURepository aPURepository,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _absencesRepository = absencesRepository;
            _submissionRepository = submissionRepository;
            _valueRepository = valueRepository;
            _categoryRepository = categoryRepository;
            _pointRepository = pointRepository;
            _attainementRepository = attainementRepository;
            _apuRepository = aPURepository;
            _env = env;
        }



        public IActionResult Index(DateTime date)
        {
            HomeViewModel homeViewModel;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                homeViewModel = GetRecapTable(date);
                return PartialView("_SubmissionsRecapPartial", homeViewModel);
            }
            else
            {
                date = DateTime.Today.Date;
                homeViewModel = GetRecapTable(date);
                return View(homeViewModel);
            }
        }


        public IActionResult IndexDetails()
        {
            //DateTime date= DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime date = DateTime.Today.Date;
            IEnumerable<Category> categories;
            Dictionary<string, List<Point>> pointCategoryList = new Dictionary<string, List<Point>>();
            Dictionary<string, List<Value>> valuesPoint = new Dictionary<string, List<Value>>();
            categories = _categoryRepository.AllCategories.Reverse();

            //get attainement Average
            var ListofAttainementAPUAverages = new Dictionary<string, List<Attainement>>();
            
            foreach (var category in categories)
            {
                var points = _pointRepository.GetPointsByCategory(category.CategoryID);
                pointCategoryList.Add(category.Category_Name, points);
                foreach (var item in points)
                {
                    valuesPoint.Add(item.Point_Name, _valueRepository.GetValuesByPoint_SubmissionDate(item.PointID, date));
                }
            }
            if (_attainementRepository.isAttainementsExist(date))
            {
                var apus = _apuRepository.AllApu;
                foreach (var ap in apus)
                {
                    var AttainementListAverage = new List<Attainement>();
                    foreach (var att in _attainementRepository.GetProjectList(ap))
                    {
                        AttainementListAverage.Add(_attainementRepository.GetAttainementsAverage(att, date));
                    }
                    ListofAttainementAPUAverages.Add(ap.APU_Name, AttainementListAverage);
                }

                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel(pointCategoryList, valuesPoint, ListofAttainementAPUAverages);
                return View(homeDetailsViewModel);
            }
            else
            {
                HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel(pointCategoryList, valuesPoint);
                return View(homeDetailsViewModel);
            }

        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByNameAsync(model.Username) == null)
                {
                    var user = new User { Name = model.Name, UserName = model.Username, Email = model.Email, IsAdmin = model.IsAdmin, Departement = model.Departement };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "User was added successfully.";

                        if (user.IsAdmin)
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
                        }
                        else
                            await _userManager.AddToRoleAsync(user, "Member");



                        //return RedirectToAction(nameof(AdminController.Index), "Admin");
                        RegisterViewModel RegisterModel = new RegisterViewModel();
                        return View(RegisterModel);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User was not added.";

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "User Already Exist";
                }
            }

            return View(model);
        }


        public async Task<IActionResult> Attendance()
        {
            //chack the existance of Attendance in absence table
            var abs = _absencesRepository.GetAbsences(DateTime.Today);
            AttendanceViewModel _attendanceViewModel = new AttendanceViewModel();
            if (abs.Count()== 0)
            {
                ViewBag.Message = "the attendance sheet";
                List<User> Users;
                Users = _userRepository.GetMembers();
                //ViewBag.MyUsers = Users;

                _attendanceViewModel.IsActive = true;
                _attendanceViewModel.Users = Users;
            }
            else
            {
                ViewBag.Message = "You have already did it";
                _attendanceViewModel.IsActive = false;
            }
            return View(_attendanceViewModel);
        }

    

        [HttpPost]
        public IActionResult Attendance(AttendanceViewModel attendanceViewModel)
        {
            List<Absence> absences = new List<Absence>();
            foreach (var absent in attendanceViewModel.AttendanceStatus)
            {
                var user = _userRepository.GetByUsername(absent.username);
                _absencesRepository.AddAbsence(new Absence { Status = absent.status, User = user, date = DateTime.Today });
            }

            return RedirectToAction(nameof(AdminController.Index), "Admin");
        }


        public async Task<IActionResult> AttendanceUpdate()
        {
            var Users = _userRepository.GetMembers();
            //chack the existance of Attendance in absence table
            var absList = _absencesRepository.GetAbsences(DateTime.Today);
            var listAbsences = new List<AbsenceViewModel>();

            if (absList.Count() != 0)
            {

                foreach (var absence in absList)
                {
                    var abs = new AbsenceViewModel { username = absence.User.UserName, status = absence.Status };
                    listAbsences.Add(abs);
                }

            }
            AttendanceViewModel _attendanceViewModel = new AttendanceViewModel(listAbsences, Users, false);

            return View(_attendanceViewModel);
        }

        [HttpPost]
        public IActionResult AttendanceUpdate(AttendanceViewModel attendanceViewModel)
        {

            List<Absence> absences = new List<Absence>();
            foreach (var absent in attendanceViewModel.AttendanceStatus)
            {
                var user = _userRepository.GetByUsername(absent.username);
                _absencesRepository.UpdateAbsences(user,DateTime.Today,absent.status);
            }

            return RedirectToAction(nameof(AdminController.Index), "Admin");
        }

        [HttpGet]
        public IActionResult History(int page = 1)
        {
            int pageSize = 10;
            var submissions_page = _submissionRepository.GetAllSubmissionByPage(page, pageSize);
            var total_submissions = _submissionRepository.AllSubmission.Count();

            ListSubmissionViewModel listsubmissions = new ListSubmissionViewModel(submissions_page, total_submissions, page, pageSize);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SubmissionsAdminTable", listsubmissions);
            }
            else
            {
                return View(listsubmissions);
            }
        }


        public async Task<IActionResult> SubmissionDetails(int id)
        {
            Submission sub = _submissionRepository.GetUserSubmissionById(id);
            var SubmissionsValues = _valueRepository.GetSubmissionValue(id);
            sub.Values = SubmissionsValues;
            IEnumerable<Point> points;
            int total;
            IEnumerable<Category> categories;
            Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            Dictionary<string, Value> ValuesPoint = new Dictionary<string, Value>();
            Dictionary<string, List<Attainement>> AttainementApuList = new Dictionary<string, List<Attainement>>();
            
            if (sub.Values != null)
            {
                categories = _categoryRepository.AllCategories.Reverse();
                foreach (var category in categories)
                {
                    points = _pointRepository.GetPointsByDepartement_Category(sub.User.Departement, category.Category_Name);
                    PointCategoryList.Add(category.Category_Name, points);
                    foreach (var item in points)
                    {
                        ValuesPoint.Add(item.Point_Name, _valueRepository.GetValueBySubmissionPoint(sub.SubmissionID, item));
                    }
                }
            }

            ViewBag.User = sub.User.UserName;
            SubmissionDetailsViewModel submissionDetailsViewModel = new SubmissionDetailsViewModel(ValuesPoint, PointCategoryList);

            


            if (_attainementRepository.isAttainementsExist(sub.submission_time))
            {
                var apus = _apuRepository.AllApu;
                foreach (var ap in apus)
                {
                    var AttainementList = new List<Attainement>();
                    foreach (var att in _attainementRepository.GetProjectList(ap))
                    {
                        AttainementList.Add(_attainementRepository.GetAttainementsAverage(att, sub.submission_time));
                    }
                    AttainementApuList.Add(ap.APU_Name, AttainementList);
                }

               submissionDetailsViewModel.ListofAttainement= AttainementApuList;
            }

            return View(submissionDetailsViewModel);

        }



        public IActionResult Dashbord() {

            var Users = _userRepository.GetMembers();
            var submissions = _submissionRepository.AllSubmission;
            List<SubmissionsStatusViewModel> submissionsStatusViewModels= new List<SubmissionsStatusViewModel>();
            List<AttendanceChartViewModel> attendanceChartViewModels= new List<AttendanceChartViewModel>();
            Dictionary<string,int> usersDepartementCount = new Dictionary<string, int>();
            //SubmissionsStatusViewModel submissionsStatusViewModel = new SubmissionsStatusViewModel();
            foreach(var user in Users)
            {
                var Late_Count = _submissionRepository.GetStatusCountByUser("Late", user);
                var OnTime_Count = _submissionRepository.GetStatusCountByUser("On time", user);
                var Missed_Count = _userRepository.GetUsersMissedSubmissions(user);

                var LateAbsCount = _absencesRepository.GetAbcencesCountByStatus_User("late", user);
                var PresentAbsCount = _absencesRepository.GetAbcencesCountByStatus_User("present", user);
                var AbsentAbsCount = _absencesRepository.GetAbcencesCountByStatus_User("absent", user);
                var DelegatedCount = _absencesRepository.GetAbcencesCountByStatus_User("delegated", user);
                submissionsStatusViewModels.Add(new SubmissionsStatusViewModel
                {
                    Username = user.Name,
                    LateCount = Late_Count,
                    OntimeCount = OnTime_Count,
                    MissedCount = Missed_Count
                });
                attendanceChartViewModels.Add(new AttendanceChartViewModel
                {
                    Username = user.Name,
                    LateCount = LateAbsCount,
                    PresentCount = PresentAbsCount,
                    AbsentCount = AbsentAbsCount,
                    DelegatedCount = DelegatedCount
                });
                
            }
            usersDepartementCount.Add("WH", _userRepository.GetDepartementUsersCount("WH"));
            usersDepartementCount.Add("CS_PP", _userRepository.GetDepartementUsersCount("CS_PP"));
            usersDepartementCount.Add("Procurement", _userRepository.GetDepartementUsersCount("Procurement"));
            DashbordViewModel dashbordViewModel = new DashbordViewModel(submissionsStatusViewModels, attendanceChartViewModels,usersDepartementCount);


            return View(dashbordViewModel); 
        }



        [HttpPost]
        public IActionResult GeneratePDF(string ExportData,DateTime pdfDate) {
            if (string.IsNullOrEmpty(ExportData))
            {
                return BadRequest("HTML content cannot be null or empty.");
            }

            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {

                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(memoryStream));
                Document doc = new Document(pdfDoc, PageSize.A4);
                string baseUrl;
                if (_env.IsDevelopment())
                {
                    // Use localhost URL for development environment
                    baseUrl = "https://localhost:7178";
                }
                else
                {
                    // Use domain URL for other environments
                    var request = HttpContext.Request;
                    baseUrl = $"{request.Scheme}://{request.Host}";
                }

                ConverterProperties converterProperties = new ConverterProperties();
                converterProperties.SetBaseUri(baseUrl);
                HtmlConverter.ConvertToPdf(ExportData, pdfDoc, converterProperties);
                pdfDoc.Close();
                pdfBytes = memoryStream.ToArray();
            }
            return File(pdfBytes, "application/pdf", "Meeting Report ("+pdfDate.ToString("M")+").pdf");
           // return File(pdfBytes, "application/pdf", "Meeting Report ("+DateTime.Today.Date.ToString("M")+").pdf");

        }


        public HomeViewModel GetRecapTable(DateTime date)
        {
            HomeViewModel homeViewModel;
            IEnumerable<Category> categories;
            Dictionary<string, List<Point>> pointCategoryList = new Dictionary<string, List<Point>>();
            //list des sommes
            var ListOfSumOfValuesOfPoints = new Dictionary<string, int>();
            //comments concatenation 
            var CommentsConcatenation = new Dictionary<string, List<string>>();
            //Descriptions concatenation 
            var DescriptionsConcatenation = new Dictionary<string, List<string>>();
            //get attainement Average
            var ListofAttainementAPUAverages = new Dictionary<string, List<Attainement>>();

            var augmentation_Status = new Dictionary<string, string>();

            categories = _categoryRepository.AllCategories.Reverse();
            foreach (var category in categories)
            {
                var points = _pointRepository.GetPointsByCategory(category.CategoryID);
                pointCategoryList.Add(category.Category_Name, points);
                foreach (var item in points)
                {
                    ListOfSumOfValuesOfPoints.Add(item.Point_Name, _valueRepository.GetSumOfValuesByPoints_SubmissionDate(item.PointID, date));
                    CommentsConcatenation.Add(item.Point_Name, _valueRepository.GetCommentsConcatenations(item, date));
                    DescriptionsConcatenation.Add(item.Point_Name, _valueRepository.GetDescriptionsConcatenations(item, date));
                    if (_valueRepository.GetSumOfValuesByPoints_SubmissionDate(item.PointID, date) > _valueRepository.GetSumOfValuesByPoints_SubmissionDate(item.PointID, date.AddDays(-1)))
                    {
                        augmentation_Status.Add(item.Point_Name, "Up");
                    }
                    else if (_valueRepository.GetSumOfValuesByPoints_SubmissionDate(item.PointID, date) == _valueRepository.GetSumOfValuesByPoints_SubmissionDate(item.PointID, date.AddDays(-1)))
                    {
                        augmentation_Status.Add(item.Point_Name, "Same");
                    }
                    else 
                    {
                        augmentation_Status.Add(item.Point_Name, "Down");
                    }
                }

            }
            if (_attainementRepository.isAttainementsExist(date))
            {
                var apus = _apuRepository.AllApu;
                foreach (var ap in apus)
                {
                    var AttainementListAverage = new List<Attainement>();
                    foreach (var att in _attainementRepository.GetProjectList(ap))
                    {
                        AttainementListAverage.Add(_attainementRepository.GetAttainementsAverage(att, date));
                    }
                    ListofAttainementAPUAverages.Add(ap.APU_Name, AttainementListAverage);
                }

                homeViewModel = new HomeViewModel(pointCategoryList, ListOfSumOfValuesOfPoints, CommentsConcatenation, DescriptionsConcatenation, ListofAttainementAPUAverages, augmentation_Status);
                //return View(homeViewModel);
            }
            else
            {
                homeViewModel = new HomeViewModel(pointCategoryList, ListOfSumOfValuesOfPoints, CommentsConcatenation, DescriptionsConcatenation, augmentation_Status);
                //return View(homeViewModel);
            }



            return homeViewModel;
        }

    }
}
