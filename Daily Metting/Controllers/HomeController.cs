using Daily_Metting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Daily_Metting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(ILogger<HomeController> logger ,SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                if (_signInManager.IsSignedIn(User))
                {

                    if (user.IsAdmin)
                        return RedirectToAction(nameof(AdminController.Index), "Admin");
                    else
                        return RedirectToAction(nameof(MemberController.Index), "Member");
                }
            }
            return RedirectToAction(nameof(AccountController.Login), "Account");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}