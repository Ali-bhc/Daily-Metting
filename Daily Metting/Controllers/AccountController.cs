using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Daily_Metting.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //try
            //{
            
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            if (user.IsAdmin)
                                return RedirectToAction(nameof(AdminController.Index), "Admin");
                            else
                                return RedirectToAction(nameof(MemberController.Index), "Member");
                        }
                        else
                        {
                            ViewBag.Error = "invalid incredentials";
                        }
                    }
                    else
                    {
                        ViewBag.Error = "invalid incredentials";
                    }
                }
         
            
            return View(model);
            
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            
                return RedirectToAction(nameof(AccountController.Login), "Account");
        }

    }

}
