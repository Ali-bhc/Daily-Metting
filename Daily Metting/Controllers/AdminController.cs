using Daily_Metting.Models;
using Daily_Metting.Repositories;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Daily_Metting.Repositories;

namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IAbsencesRepository _absencesRepository;


        public AdminController(UserManager<User> userManager,IUserRepository userRepository , IAbsencesRepository absencesRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _absencesRepository = absencesRepository;
        }
        public IActionResult Index()
        {
            return View();
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

                var user = new User {Name = model.Name, UserName = model.Username, Email = model.Email, IsAdmin = model.IsAdmin, Departement = model.Departement };
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

            return View(model);
        }



        public IActionResult Attendance()
        {
            IEnumerable<User> Users;

            Users = _userRepository.AllUsers;
            ViewBag.MyUsers = Users;

            //string abs = "";
            //User user= new User();
            //AbsenceViewModel absenceViewModel = new AbsenceViewModel(abs,user.UserName);
            //List<AbsenceViewModel> absenceViewModels= new List<AbsenceViewModel>();

            //AttendanceViewModel attendanceViewModel = new AttendanceViewModel();
            return View(new AttendanceViewModel());
        }


        [HttpPost]
        public IActionResult Attendance(AttendanceViewModel attendanceViewModel)
        {
            List<Absence> absences = new List<Absence>();
            foreach(var absent in attendanceViewModel.AttendanceStatus)
            {
                var user = _userRepository.GetByUsername(absent.username);
                _absencesRepository.AddAbsence(new Absence { Status = absent.status, User = user, date = DateTime.Now });
            }

            return RedirectToAction(nameof(AdminController.Index), "Admin");
        }

    }
}
