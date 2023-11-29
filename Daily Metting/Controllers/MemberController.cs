using Daily_Metting.Repositories.Absences;
using Daily_Metting.Repositories.Apus;
using Daily_Metting.Repositories.Attainements;
using Daily_Metting.Repositories.Categories;
using Daily_Metting.Repositories.Points;
using Daily_Metting.Repositories.Submissions;
using Daily_Metting.Repositories.Users;
using Daily_Metting.Repositories.Values;
using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using ExcelDataReader;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
//using iText.StyledXmlParser.Jsoup.Nodes;
//using iTextSharp.text.log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Point = Daily_Metting.Models.Point;


namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "MemberPolicy")]
    public class MemberController : Controller
    {
        private readonly IValueRepository _valueRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPointRepository _pointRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IAttainementRepository _attainementRepository;
        private readonly IAPURepository _apuRepository;
        private readonly IAbsencesRepository _absencesRepository;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;


        public MemberController(IValueRepository valueRepositoryy, ICategoryRepository categoryRepository, IPointRepository pointRepository,
            ISubmissionRepository submissionRepository, IAPURepository apuRepository, IAttainementRepository attainementRepository,
            IAbsencesRepository absencesRepository, IUserRepository userRepository,
            SignInManager<User> signInManager, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _valueRepository = valueRepositoryy;
            _categoryRepository = categoryRepository;
            _pointRepository = pointRepository;
            _submissionRepository = submissionRepository;
            _apuRepository = apuRepository;
            _attainementRepository = attainementRepository;
            _absencesRepository= absencesRepository;  
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
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

        [HttpPost]
        public IActionResult GeneratePDF(string ExportData)
        {
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
            return File(pdfBytes, "application/pdf", "Meeting Report (" + DateTime.Today.Date.ToString("M") + ").pdf");

        }



        public async Task<IActionResult> UploadSubmission()
        {
            var user = await _userManager.GetUserAsync(User);
            bool IsSubmitted=false;
            bool IsMissed=false;
            // First we should check if the user already submit
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Now.Date, user);

            string submission_status;
            DateTime currentTime = DateTime.Now;

            // Get the time today at 10:45 AM
            DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 10, 30, 0);
            DateTime MissingTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 23, 00, 0);


            // Compare the current time with the target time
            if (currentTime >= targetTime && currentTime < MissingTime)
            {
                submission_status = "Late";
            }
            else if (currentTime < targetTime)
            {
                submission_status = "On time";
            }
            else
            {
                submission_status = "Missed";
                IsMissed=true;
            }


            if (sub == null && submission_status != "Missed")
            {
                ViewBag.Message = "Hello " + user.ToString() + " DO your submission for " + DateTime.Today.Date.ToString("M") + ":";
            }
            else if (sub != null && submission_status != "Missed")
            {
                ViewBag.Message = "Hello " + user.ToString() + " you have already submit for today! \n ";
                IsSubmitted= true;
            }
            else
            {
                ViewBag.Message = "Hello " + user.ToString() + " you can not Add or Update Today's submission , try tomorrow! ";
            }
            UploadSubmissionViewModel uploadSubmissionViewModel = new UploadSubmissionViewModel(IsSubmitted,user.Departement,IsMissed);
            return View(uploadSubmissionViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> UploadSubmission([FromForm] UploadSubmissionViewModel file)
        {
            //get the user
            var user = await _userManager.GetUserAsync(User);
            bool Is_CS_PP_User = user.Departement == "CS_PP" ? true : false;

            if (FileValidation(file.File.FileName, user.Departement))
            {
                try
                {
                    var submission = GetCurrentSubmission(user);
                    if (submission != null)
                    {
                        using (var stream = file.File.OpenReadStream())
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                UploadValuesFile(reader, submission);
                                if (Is_CS_PP_User)
                                {
                                    reader.NextResult();
                                    UploadAttainementFile(reader, submission);
                                }
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    var uploadViewModel = new UploadSubmissionViewModel(false,user.Departement,"the excel file has been modified");
                    return View(uploadViewModel);
                }
            }

            return RedirectToAction("Index");

        }



        public async Task<IActionResult> AddSubmission()
        {
            var user = await _userManager.GetUserAsync(User);
            Dictionary<string,List<string>> ProjectList= new Dictionary<string,List<string>>();
            int total;
            IEnumerable<Category> categories;
            List<APU> Apus ;
            Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            // First we should check if the user already submit
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Now.Date, user);

            string submission_status;
            DateTime currentTime = DateTime.Now;

            // Get the time today at 10:45 AM
            DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 11, 15, 0);
            DateTime MissingTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 00, 0);


            // Compare the current time with the target time
            if (currentTime >= targetTime && currentTime < MissingTime)
            {
                submission_status = "Late";
            }
            else if (currentTime < targetTime)
            {
                submission_status = "On time";
            }
            else
                submission_status = "Missed";

            if (sub == null && submission_status != "Missed")
            {
                ViewBag.Message = "Hello " + user.ToString() + " DO your submission for " + DateTime.Today.Date.ToString("M") + ":";
                categories = _categoryRepository.AllCategories.Reverse();
                foreach (var category in categories)
                {
                    PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
                }

                if (user.Departement != "CS_PP")
                {
                    ////List<ValueViewModel> values = new List<ValueViewModel>();
                    //total = PointCategoryList.Values.Sum(list => list.Count());

                    SubmissionViewModel _submissionViewModel = new SubmissionViewModel(PointCategoryList,false,false);
                    return View(_submissionViewModel);
                }
                else
                {
                    Apus = _apuRepository.AllApu;
                    foreach (var apu in Apus)
                    {
                        ProjectList.Add(apu.APU_Name, _attainementRepository.GetProjectList(apu,DateTime.Today.Date));
                    }
                    SubmissionViewModel _submissionViewModel = new SubmissionViewModel(PointCategoryList,ProjectList, false,true);
                    return View(_submissionViewModel);
                }

                ////List<ValueViewModel> values = new List<ValueViewModel>();
                //total = PointCategoryList.Values.Sum(list => list.Count());

                //SubmissionViewModel _submissionViewModel = new SubmissionViewModel(PointCategoryList, total, false);
                //return View(_submissionViewModel);
            }
            else if(sub != null && submission_status != "Missed")
            {
                ViewBag.Message = "Hello " + user.ToString() + " you have already submit for today! \nGo check it ";
                SubmissionViewModel _submissionViewModel = new SubmissionViewModel(true,false);
                return View(_submissionViewModel);
            }
            else
            {
                ViewBag.Message = "Hello " + user.ToString() + " you can not Add or Update Today's submission , try tomorrow! ";
                SubmissionViewModel _submissionViewModel = new SubmissionViewModel(false,true);
                return View(_submissionViewModel);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddSubmission( SubmissionViewModel _submissionViewModel)
        {
            //get the user
            var user = await _userManager.GetUserAsync(User);
            string submission_status;
            bool Is_CS_PP_User = user.Departement == "CS_PP" ? true : false;
            var submission = GetCurrentSubmission(user);

            //Step2:Insert Values from the Model to Database
            foreach (var key in _submissionViewModel.Values.Keys)
            {

                //foreach (var val in _submissionViewModel.Values[key])
                //{
                    if (_submissionViewModel.Values[key].PointID != 0)
                    {
                        var pt = _pointRepository.GetByID(_submissionViewModel.Values[key].PointID);
                        _valueRepository.AddSubmissionValue(new Value { 
                            Value_point = _submissionViewModel.Values[key].Value_point, 
                            description = _submissionViewModel.Values[key].description, 
                            comment = _submissionViewModel.Values[key].comment, 
                            Point = pt, 
                            Submission = submission });
                    }
                //}

            }
            if (Is_CS_PP_User)
            {
                var Apus = _apuRepository.AllApu;
                var ProjectList = new Dictionary<string,List<string>>();
                foreach (var apu in Apus)
                {
                    ProjectList.Add(apu.APU_Name, _attainementRepository.GetProjectList(apu,submission.submission_time.Date));
                }
                foreach(var apu in ProjectList.Keys)
                {
                    foreach (var att in ProjectList[apu])
                    {
                            var Apu = _apuRepository.GetAPUByName(apu);
                            var attainement = new Attainement
                            {
                                APU = Apu,
                                Project_name = _submissionViewModel.AttainamentsList[att].Project_name,
                                Attainement_OTIF = _submissionViewModel.AttainamentsList[att].Attainement_OTIF,
                                Attainement_Mix = _submissionViewModel.AttainamentsList[att].Attainement_Mix,
                                Productivity = _submissionViewModel.AttainamentsList[att].Productivity,
                                Downtime = _submissionViewModel.AttainamentsList[att].Downtime,
                                Scrap = _submissionViewModel.AttainamentsList[att].Scrap,
                                Comment = _submissionViewModel.AttainamentsList[att].Comment,
                                Submission = submission
                            };
                            _attainementRepository.AddAttainement(attainement);
                        }
                    }
                }

            return RedirectToAction(nameof(MemberController.Index), "Member");
        }


        public async Task<IActionResult> UpdateSubmission()
        {
            var user = await _userManager.GetUserAsync(User);
            IEnumerable<Point> points;
            IEnumerable<Category> categories;
            List<Point> UserPoints = _pointRepository.GetPointsByDepartement(user.Departement);
            Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            Dictionary<string, ValueViewModel> valueslist = new Dictionary<string,ValueViewModel>();
            Dictionary<string, AttainementsViewModel> Attainementslist = new Dictionary<string, AttainementsViewModel>();

            // First we should check if the user already submit
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Now.Date, user);
            //var values = _valueRepository.GetSubmissionValue(sub.SubmissionID);
  

            //get the submission value
            if (sub != null )
            {

                foreach (var point in UserPoints)
                {
                    //var listofvalues = new Dictionary<string,ValueViewModel>();
                    var value = _valueRepository.GetValueBySubmissionPoint(sub.SubmissionID, point);
                    //foreach (var val in values)
                    //{
                    //    var pt = _pointRepository.GetByID(val.Point.PointID);
                    //    listofvalues.Add("point",new ValueViewModel { Value_point = val.Value_point, description = val.description, comment = val.comment, PointID = pt.PointID });
                    //}
                    valueslist.Add(point.Point_Name, new ValueViewModel { 
                        Value_point = value.Value_point, 
                        description = value.description, 
                        comment = value.comment, 
                        PointID = point.PointID });
                }

                categories = _categoryRepository.AllCategories.Reverse();
                foreach (var category in categories)
                {
                    PointCategoryList.Add(category.Category_Name, _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name));
                }
                if (user.Departement == "CS_PP")
                {
                    var ProjectList = new Dictionary<string, List<string>>();
                    var Apus = _apuRepository.AllApu;
                    foreach (var apu in Apus)
                    {
                        ProjectList.Add(apu.APU_Name, _attainementRepository.GetProjectList(apu,sub.submission_time.Date));
                    }
                    foreach (var attainement in _attainementRepository.GetAttainementsBySubmission(sub))
                    {

                        var att = new AttainementsViewModel
                        {
                            Project_name = attainement.Project_name,
                            Attainement_OTIF = attainement.Attainement_OTIF,
                            Attainement_Mix = attainement.Attainement_Mix,
                            Downtime = attainement.Downtime ,
                            Scrap = attainement.Scrap,
                            Productivity = attainement.Productivity,
                            Comment = attainement.Comment,
                            ApuName = attainement.APU.APU_Name
                        };
                        Attainementslist.Add(attainement.Project_name, att);
                    }

                    return View(new SubmissionViewModel(valueslist, PointCategoryList, Attainementslist,ProjectList,true));
                    //return View(new SubmissionViewModel { Values = valueslist, PointCategoryList = PointCategoryList, AttainamentsList = Attainementslist, ProjectList = ProjectList, Is_CS_PP = true ,IsMissed=false});

                }
                else
                {
                    return View(new SubmissionViewModel(valueslist, PointCategoryList,false));
                    //return View(new SubmissionViewModel { Values = valueslist, PointCategoryList = PointCategoryList, Is_CS_PP = true, IsMissed = false });


                }
            }
            else
            {
                return RedirectToAction("AddSubmission");
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSubmission(SubmissionViewModel _submissionViewModel)
        {

            //get the user
            var user = await _userManager.GetUserAsync(User);
            //Verify User departement 
            bool Is_CS_PP_User = user.Departement == "CS_PP" ? true : false;
            //get the submission
            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Now.Date, user);

            //delete old attainements
            _attainementRepository.DeleteAttainementsBySubmission(sub);
            //delete old values
            _valueRepository.DeleteValuesBySubmission(sub);

            //delete submission
            _submissionRepository.DeleteSubmission(sub);

            var submission = new Submission
            {
                User = user, // Set the user property here
                submission_time = sub.submission_time, // Set the date property here
                status = sub.status

            };

            _submissionRepository.AddSubmission(submission);
            //Step2:Insert Values from the Model to Database
            foreach (var key in _submissionViewModel.Values.Keys)
            {

                var valueslist = new Dictionary<string,ValueViewModel>();
                var pt = _pointRepository.GetByID(_submissionViewModel.Values[key].PointID);
                _valueRepository.AddSubmissionValue(new Value {
                    Value_point = _submissionViewModel.Values[key].Value_point, 
                    description = _submissionViewModel.Values[key].description, 
                    comment = _submissionViewModel.Values[key].comment, 
                    Point = pt, Submission = submission 
                });

            }

            if (Is_CS_PP_User)
            {
                var Apus = _apuRepository.AllApu;
                var ProjectList = new Dictionary<string, List<string>>();
                foreach (var apu in Apus)
                {
                    ProjectList.Add(apu.APU_Name, _attainementRepository.GetProjectList(apu,submission.submission_time.Date));
                }
                foreach (var apu in ProjectList.Keys)
                {
                    foreach (var att in ProjectList[apu])
                    {
                        var Apu = _apuRepository.GetAPUByName(apu);
                        var attainement = new Attainement
                        {
                            APU = Apu,
                            Project_name = _submissionViewModel.AttainamentsList[att].Project_name,
                            Attainement_OTIF = _submissionViewModel.AttainamentsList[att].Attainement_OTIF/100,
                            Attainement_Mix = _submissionViewModel.AttainamentsList[att].Attainement_Mix / 100,
                            Productivity = _submissionViewModel.AttainamentsList[att].Productivity / 100,
                            Downtime = _submissionViewModel.AttainamentsList[att].Downtime / 100,
                            Scrap = _submissionViewModel.AttainamentsList[att].Scrap / 100,
                            Comment = _submissionViewModel.AttainamentsList[att].Comment,
                            Submission = submission
                        };
                        _attainementRepository.AddAttainement(attainement);
                    }
                }
            }


            return RedirectToAction(nameof(MemberController.Index), "Member");
        }


        [HttpGet]
        public async Task<IActionResult> History(int page = 1)
        {
            int pageSize = 10;
            var user = await _userManager.GetUserAsync(User);
            var submissions_page = _submissionRepository.GetUserSubmissionByPage(page, pageSize, user);
            var total_submissions = _submissionRepository.GetUserSubmission(user).Count();

            ListSubmissionViewModel listsubmissions = new ListSubmissionViewModel(submissions_page, total_submissions, page, pageSize);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SubmissionsTable", listsubmissions);
            }
            else
            {
                return View(listsubmissions);
            }
        }


        public async Task<IActionResult> SubmissionDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            //Submission sub = _submissionRepository.GetUserSubmissionById(id);
            //var SubmissionsValues = _valueRepository.GetSubmissionValue(id);
            //sub.Values = SubmissionsValues;
            //IEnumerable<Point> points;
            //int total;
            //IEnumerable<Category> categories;
            //Dictionary<string, IEnumerable<Point>> PointCategoryList = new Dictionary<string, IEnumerable<Point>>();
            //Dictionary<string, Value> ValuesPoint = new Dictionary<string, Value>();
            //categories = _categoryRepository.AllCategories.Reverse();
            //foreach (var category in categories)
            //{
            //    points = _pointRepository.GetPointsByDepartement_Category(user.Departement, category.Category_Name);
            //    PointCategoryList.Add(category.Category_Name, points);
            //    foreach (var item in points)
            //    {
            //        ValuesPoint.Add(item.Point_Name, _valueRepository.GetValueBySubmissionPoint(sub.SubmissionID, item));
            //    }
            //}
            //SubmissionDetailsViewModel submissionDetailsViewModel = new SubmissionDetailsViewModel(ValuesPoint, PointCategoryList);
            //return View(submissionDetailsViewModel);
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


            if (user.Departement == "CS_PP")
            {

                if (_attainementRepository.isAttainementsExist(sub.submission_time))
                {
                    var apus = _apuRepository.AllApu;
                    foreach (var ap in apus)
                    {
                        //var AttainementList = new List<Attainement>();
                        var AttainementList = _attainementRepository.GetAttainementsBySubmission_Apu(sub,ap);

                        //foreach (var att in _attainementRepository.GetProjectList(ap))
                        //{
                        //    AttainementList.Add(_attainementRepository.GetAttainementsAverage(att, sub.submission_time));
                        //}
                        AttainementApuList.Add(ap.APU_Name, AttainementList);
                    }

                    submissionDetailsViewModel.ListofAttainement = AttainementApuList;
                }
            }

            return View(submissionDetailsViewModel);


        }


        public async Task<IActionResult> UpdateSubmissionByUpload()
        {
            var user = await _userManager.GetUserAsync(User);
            var uploadSubmissionViewModel = new UploadSubmissionViewModel(true, user.Departement);
            return View(uploadSubmissionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubmissionByUpload([FromForm] UploadSubmissionViewModel file)
        {
            //get the user
            var user = await _userManager.GetUserAsync(User);
            
            //get User Departement for adding Attainement
            bool Is_CS_PP_User = user.Departement == "CS_PP" ? true : false;
            try
            {
                if (FileValidation(file.File.FileName, user.Departement))
                {
                   
                    //var submission = GetCurrentSubmission(user);
                    using (var stream = file.File.OpenReadStream())
                    {
                        using (var reader1 = ExcelReaderFactory.CreateReader(stream))
                        {
                            //if (DataValidation(reader1))
                            //{
                            //get the submission
                            var sub = _submissionRepository.GetSubmissionByUser_Date(DateTime.Now.Date, user);

                            var submission1 = new Submission
                            {
                                User = user, // Set the user property here
                                submission_time = sub.submission_time, // Set the date property here
                                status = sub.status

                            };
                            _submissionRepository.AddSubmission(submission1);


                            //delete old values
                            _valueRepository.DeleteValuesBySubmission(sub);

                            //delete old Attainements
                            _attainementRepository.DeleteAttainementsBySubmission(sub);

                            //delete submission
                            _submissionRepository.DeleteSubmission(sub);


                            UploadValuesFile(reader1, submission1);
                            if (Is_CS_PP_User)
                            {
                                reader1.NextResult();
                                UploadAttainementFile(reader1, submission1);
                            }
                        

                            //}
                            //else
                            //{
                            //    var uploadViewModel = new UploadSubmissionViewModel(true, user.Departement, "the file has been modified");
                            //    return View(uploadViewModel);
                            //}
                        }

                    }
                }
                else
                {
                    ViewBag.Message = "You Uploaded the wrong file,Please Retry !";
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                var uploadViewModel = new UploadSubmissionViewModel(true, user.Departement, "the file has been modified");
                return View(uploadViewModel);
            }

        }
       
        public void UploadValuesFile(IExcelDataReader reader,Submission submission)
        {
            int counter1 = 0;
            string description_value=string.Empty;
            string comment_value = string.Empty;
            while (reader.Read())
            {

                if (counter1 > 0)
                {
                    //Console.WriteLine(reader.GetFieldType(3));

                    if (reader.GetFieldType(3) == typeof(Double))
                    {
                        description_value = reader.GetDouble(3).ToString(); // convert number to string
                    }
                    else
                    {
                        description_value = reader.GetString(3);
                    }
                    if (reader.GetFieldType(4) == typeof(Double))
                    {
                        comment_value = reader.GetDouble(4).ToString(); // convert number to string
                    }
                    else
                    {
                        comment_value = reader.GetString(4);
                    }
                    var pointVal = (reader.GetValue(2) == null || reader.GetValue(2).ToString() == "") ? 0 : Convert.ToInt32(reader.GetValue(2));
                    //Map Excel data to database model
                    var value = new Value()
                    {
                        Point = _pointRepository.GetByName(reader.GetString(1)),
                        Value_point = pointVal,
                        //description = (string)reader.GetValue(3),
                        description = description_value,
                        comment = comment_value,
                        Submission = submission
                    };
                    // Save model to database
                    _valueRepository.AddSubmissionValue(value);
                }

                counter1++;
            }

        }

        public bool DataValidation(IExcelDataReader reader)
        {
            try
            {
                int counter1 = 0;
                while (reader.Read() || reader == null)
                {

                    if (counter1 > 0)
                    {
                        //Map Excel data to database model
                        var value = new Value()
                        {
                            Point = _pointRepository.GetByName(reader.GetString(1)),
                            Value_point = (int)reader.GetDouble(2),
                            description = reader.GetString(3),
                            comment = reader.GetString(4),
                        };
                    }

                    counter1++;
                }
                

            }
            catch (Exception ex) 
            { 
                return false; 
            }
            return true;

        }

        public void UploadAttainementFile(IExcelDataReader reader, Submission submission)
        {
            var apu = new APU();
            int counter2 = 0;
            while (reader.Read())
            {
                if (counter2 > 0)
                {
                    Console.WriteLine(reader.GetString(0));
                    //if (!reader.GetString(0).Equals(null))
                    if (reader.GetString(0) != null)
                    {
                        if (_apuRepository.GetAPUByName(reader.GetString(0)) != null)
                        {
                            apu = _apuRepository.GetAPUByName(reader.GetString(0));
                            _apuRepository.UpdateAPU(apu.Id, reader.GetDouble(1), reader.GetDouble(2));
                        }
                        else
                        {
                            apu = new APU
                            {
                                APU_Name = reader.GetString(0),
                                Attainement_min = reader.GetDouble(1),
                                Attainement_Max = reader.GetDouble(2)
                            };
                            _apuRepository.AddAPU(apu);
                        }
                    }

                    APU ap = apu;
                    var Project_name = reader.GetString(3);
                    var Attainement_OTIF = (double)reader.GetDouble(4);
                    var Attainement_Mix = (double)reader.GetDouble(5);
                    var Productivity = (double)reader.GetDouble(6);
                    var Downtime = (double)reader.GetDouble(7);
                    var Scrap = (double)reader.GetDouble(8);
                    var Comment = reader.GetString(9);
                    var Submission = submission;
                    // Map Excel data to database model
                    var model = new Attainement()
                    {
                        APU = apu,
                        Project_name = reader.GetString(3),
                        Attainement_OTIF = (double)reader.GetDouble(4),
                        Attainement_Mix = (double)reader.GetDouble(5),
                        Productivity = (double)reader.GetDouble(6),
                        Downtime = (double)reader.GetDouble(7),
                        Scrap = (double)reader.GetDouble(8),
                        Comment = reader.GetString(9),
                        Submission = submission
                    };


                    // Save model to database
                    _attainementRepository.AddAttainement(model);
                }
                counter2++;
            }


        }

        public bool FileValidation(string filename, string UserDepartement)
        {
            bool IsValid=false;
            switch (UserDepartement)
            {
                case "WH" : IsValid = filename.Contains("DailyMeetingWareHouseFile") ? true:false;break; 
                case "CS_PP" : IsValid = filename.Contains("DailyMeetingCS_PP_File") ? true:false;break; 
                case "Procurement": IsValid = filename.Contains("DailyMeetingProcurementFile") ? true:false;break; 
            }
            return IsValid; 
        }

        public Submission GetCurrentSubmission(User user)
        {
            string submission_status;
            DateTime currentTime = DateTime.Now;

            // Get the time today at 10:45 AM
            DateTime targetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 11, 15, 0);
            DateTime MissingTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 14, 00, 0);


            // Compare the current time with the target time
            if (currentTime >= targetTime && currentTime < MissingTime)
            {
                submission_status = "Late";
            }
            else if (currentTime < targetTime)
            {
                submission_status = "On time";
            }
            else
                submission_status = "Missed";
            
            if (submission_status != "Missed")
            {

                //Step1:Add Submission First
                var submission = new Submission
                {
                    User = user, // Set the user property here
                    submission_time = DateTime.Now, // Set the date property here
                    status = submission_status,
                };

                _submissionRepository.AddSubmission(submission);

                return submission;
            }
            return null;

        }

        private HomeViewModel GetRecapTable(DateTime date)
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
                    var testList = _attainementRepository.GetProjectList(ap,date);
                    foreach (var att in testList)
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
        
        public async Task<IActionResult> Attendance()
        {
            //chack the existance of Attendance in absence table
            var abs = _absencesRepository.GetAbsences(DateTime.Today);
            AttendanceViewModel _attendanceViewModel = new AttendanceViewModel();
            if (abs.Count() == 0)
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

            return RedirectToAction(nameof(MemberController.Index), "Member");
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update_User()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var userToUpdate = new UpdateUserViewModel { Id = user.Id, Name = user.Name, Username = user.UserName, Departement = user.Departement};
                return View(userToUpdate);
            }
            return RedirectToAction(nameof(AdminController.TeamMembers), "Admin");

        }


        [HttpPost]
        public async Task<IActionResult> Update_User(UpdateUserViewModel updateuserVM)
        {
            var user = await _userManager.FindByIdAsync(updateuserVM.Id);

            //var user = _userRepository.GetByUsername(updateuserVM.Username);
            if (user != null)
            {
                _userRepository.UpdateUser(updateuserVM);
            }
            return RedirectToAction(nameof(MemberController.Profile), "Member");

        }


        [HttpGet]
        public IActionResult ChangePassword() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction(nameof(Index));
        }


    }



}


    
