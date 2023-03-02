using Daily_Metting.Models;
using Daily_Metting.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AdminController(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}
