using DataAccess;
using Library.FileInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        private readonly IWebHostEnvironment _webHost;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager, TennisContext context, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _webHost = webHost;
        }
        [Route("Admin")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();

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
            bool a1;
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
                    ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại!");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName,
                                    model.Password, model.RememberMe, false); // Không apply LOGOUT feature nên set giá trị false
                if (result.Succeeded)
                {
                    a1 = await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Manager") || await _userManager.IsInRoleAsync(user, "Referee");
                    //switch (a1)
                    //{
                    //    case true:
                    //        return RedirectToAction("Index", "Match", new { isCurrent = true });
                    //    default:
                    //        return RedirectToAction("Index", "Home", new { area = "NoRole" }); // Redirect to Players Index page
                    //}
                    if (a1) { return RedirectToAction("Index", "Match", new { isCurrent = true }); } 
                    else { return RedirectToAction("Index", "Home", new { area = "NoRole" }); }// Redirect to Players Index page
                }
                ModelState.AddModelError(string.Empty, "Đăng nhập không thành công!");
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
                    var result1 = await _roleManager.FindByNameAsync("Admin"); // Check for "Admin" role in DB
                    var result2 = await _roleManager.FindByNameAsync("Manager"); // Check for "Manager" role in DB
                    var result3 = await _roleManager.FindByNameAsync("Referee"); // Check for "Referee" role in DB
                    bool upfail = false;

                    new Initializer(_webHost).Special1stRoundGenerator();

                    if (result1 == null)  // No "Admin" role found
                    {
                        // Create new role
                        var newRole = new IdentityRole("Admin");
                        var rsNewRole = await _roleManager.CreateAsync(newRole);
                        upfail = !rsNewRole.Succeeded;
                    }
                    if (result2 == null)  // No "Manager" role found
                    {
                        // Create new role
                        var newRole = new IdentityRole("Manager");
                        var rsNewRole = await _roleManager.CreateAsync(newRole);
                        upfail = upfail && rsNewRole.Succeeded;
                    }
                    if (result3 == null)  // No "Referee" role found
                    {
                        // Create new role
                        var newRole = new IdentityRole("Referee");
                        var rsNewRole = await _roleManager.CreateAsync(newRole);
                        upfail = upfail && rsNewRole.Succeeded;
                    }
                    if (upfail) // Không tạo đủ Role
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
                    await _userManager.AddToRoleAsync(user, "Admin");
                    if (result.Succeeded)
                    {
                        var rs = await _signInManager.PasswordSignInAsync(user.UserName,
                                    model.Password, false, false);
                        if (rs.Succeeded)
                        {
                            return RedirectToAction("Index", "Match", new { isCurrent = true });
                        }
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
        public IActionResult Index()
        {
            var model = _userManager.Users.ToList();
            return View(model);
        }
        public async Task<IActionResult> Update(string id)
        {
            string selectedValue = null;
            var model = new AccountViewModel();
            if(id != null)
            {
                var user = await _userManager.FindByIdAsync(id);
                model.Username = user.UserName;
                model.Email = user.Email;
                model.Note = user.Ghi_Chu;
                model.FullName = user.FullName;
                var roles = await _userManager.GetRolesAsync(user);
                selectedValue = roles[0];
            }
            ViewBag.RoleName = new SelectList(_roleManager.Roles, "Name", "Name", selectedValue);
            ViewBag.Id = id;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, AccountViewModel model)
        {
            var user = new AppUser();
            if (id != null) user = await _userManager.FindByIdAsync(id);
            user.FullName = model.FullName;
            user.UserName = model.Username;
            user.Email = model.Email;
            user.Ghi_Chu = model.Note;
            var result = new IdentityResult();
            if (id == null) 
            { 
                await _userManager.CreateAsync(user, model.Password);
                result = await _userManager.AddToRoleAsync(user, model.RoleName);
            }
            else
            {
                await _userManager.UpdateAsync(user);
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                result = await _userManager.AddToRoleAsync(user, model.RoleName);
            }
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, AccountViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, model.Password);
            return RedirectToAction(nameof(Update), new { isNew = false, id });
        }
        public IActionResult Delete(string id)
        {
            //var user = await _userManager.FindByIdAsync(id);
            //await _userManager.DeleteAsync(user);
            var user = _context.AppUsers.Find(id);
            _context.Remove(user);
            _context.SaveChangesAsync();
            return RedirectToAction("Index", "Account");
        }
    }
}
