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
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if(user.IsAdmin)
                        return RedirectToAction(nameof(AdminController.Index), "Admin");
                    else
                        return RedirectToAction(nameof(MemberController.Index), "Member");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(model);
            //var user = await _userManager.FindByEmailAsync(model.Email);

            //if (user != null)
            //{
            //    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, model.RememberMe);

            //    if (result.Succeeded)
            //    {
            //        await _signInManager.SignInAsync(user, model.RememberMe);
            //    }

            //    return result;
            //}

            //return SignInResult.Failed;
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //if (user != null)
            //{ 
            //    if(user.IsAdmin)
            //    {
            //        return RedirectToAction(nameof(AdminController.Index), "Admin");
            //    }
            //    else
            //        return RedirectToAction(nameof(MemberController.Index), "Member");

            //}
            //else
                return RedirectToAction(nameof(AccountController.Login), "Account");
        }

    }

}
