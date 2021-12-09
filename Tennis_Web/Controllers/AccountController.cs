using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TennisContext _context;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, TennisContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        //[Route("Admin")]
        //[Route("Account/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            if (!_userManager.Users.Any())
            {
                return ViewComponent(MessagePage.COMPONENTNAME,
                    new MessagePage.Message()
                    {
                        Title = "Thông báo",
                        Secondwait = 5,
                        Htmlcontent = "Hệ thống chưa có người quản trị. Chuyển sang đăng ký mới người quản trị hệ thống",
                        Urlredirect = Url.Action("Register", "Account")
                    });
            }
            return View();
        }
        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool a1, a2, a3;
            if (ModelState.IsValid)
            {
                // Find user by Emaik
                AppUser user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                // Find user by Username
                if (user == null)
                    user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
                // Return error if not found
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName,
                                    model.Password, model.RememberMe, false); // Không apply LOGOUT feature nên set giá trị false
                if (result.Succeeded)
                {
                    //a1 = await _userManager.IsInRoleAsync(user, "Admin");
                    //a2 = await _userManager.IsInRoleAsync(user, "Manager");
                    //a3 = await _userManager.IsInRoleAsync(user, "Referee");
                    a1 = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Manager") || await _userManager.IsInRoleAsync(user, "Referee");
                    switch (a1)
                    {
                        //case (true, true or false, true or false):
                        //    return RedirectToAction("Index", "Admin"); 
                        //case (false, true, true or false):
                        //    return RedirectToAction("Index", "Manager"); 
                        //case (false, false, true):
                        //    return RedirectToAction("Index", "Referee"); 
                        case (true):
                            return RedirectToAction("Index", "Home");
                        default:
                            return RedirectToAction("Index", "Home", new { area = "NoRole" }); // Redirect to Players Index page
                    }
                }
                ModelState.AddModelError(string.Empty, "Đăng nhập không thành công");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                if (!_userManager.Users.Any())
                {
                    // Copy data from ViewModel to IdentityUser
                    var user = new AppUser
                    {
                        FullName = model.FullName,
                        Email = model.Email,
                        UserName = "Admin"
                    };

                    // Store user data in Users database table
                    var result = await _userManager.CreateAsync(user, model.Password);
                    var result2 = await _roleManager.FindByNameAsync("Admin"); // Check for "Admin" role in DB
                    if (result2 == null)  // No "Admin" role found
                    {
                        // Create new role
                        var newRole = new IdentityRole("Admin");
                        var rsNewRole = await _roleManager.CreateAsync(newRole);
                        if (!rsNewRole.Succeeded)
                        {
                            // Display error and return to default index page if Creating "Admin" is unsuccessful
                            return ViewComponent(MessagePage.COMPONENTNAME,
                            new MessagePage.Message()
                            {
                                Title = "Thông báo",
                                Secondwait = 60,
                                Htmlcontent = "Lỗi hệ thống. Trở về mặc định",
                                Urlredirect = Url.Action("Index", "Home", new { area = "NoRole" })
                            });
                        }
                    }
                    await _userManager.AddToRoleAsync(user, "Admin");

                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View(model);
            }
            return ViewComponent(MessagePage.COMPONENTNAME,
                new MessagePage.Message()
                {
                    Title = "Thông báo",
                    Secondwait = 60,
                    Htmlcontent = "Lỗi hệ thống. Trở về mặc định",
                    Urlredirect = Url.Action("Index", "Home", new { area = "NoRole" })
                });
        }
    }
}
