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
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            bool a1, a2, a3;
            if (ModelState.IsValid)
            {
                // Tìm user bằng Email
                AppUser user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
                // Tìm user  bằng Username
                if (user == null)
                    user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
                // Báo lỗi nếu không tìm ra
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại");
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName,
                                    model.Password, model.RememberMe, false); // Không apply LOGOUT feature nên set giá trị false
                if (result.Succeeded)
                {
                    a1 = await _userManager.IsInRoleAsync(user, "Admin");
                    a2 = await _userManager.IsInRoleAsync(user, "Manager");
                    a3 = await _userManager.IsInRoleAsync(user, "Referee");
                    switch (a1, a2, a3)
                    {
                        case (true, true or false, true or false):
                            return RedirectToAction("Index", "Admin"); // Redirect tới trang chủ của Admin
                        case (false, true, true or false):
                            return RedirectToAction("Index", "Manager"); // Redirect tới trang chủ của Manager
                        case (false, false, true):
                            return RedirectToAction("Index", "Referee"); // Redirect tới trang chủ của Referee
                        default:
                            return RedirectToAction("Index", "Home", new { area = "NoRole" }); // Redirect tới trang chủ của VDV
                    }
                }
                ModelState.AddModelError(string.Empty, "Đăng nhập không thành công");
            }
            return View(model);
        }
    }
}
