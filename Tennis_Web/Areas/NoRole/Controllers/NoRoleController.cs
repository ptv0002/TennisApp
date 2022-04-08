using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Models;
using Tennis_Web.Areas.NoRole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Library;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class NoRoleController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly INotyfService _notyf;
        public NoRoleController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        //      public async Task<IActionResult> Index()
        public IActionResult Index()
        {
            //new Initializer(_webHost).Special1stRoundGenerator();

            return View();
        }
        public IActionResult HotNews()
        {
            var model = _context.Thong_Baos.Where(m => m.Hien_Thi && m.Tin_Nong).OrderByDescending(m => m.Ngay).ToList();
            return View(model);
        }
        public IActionResult Announcement(bool isCurrent)
        {
            var model = _context.Thong_Baos.Where(m => m.DS_Giai.Giai_Moi == isCurrent && m.Hien_Thi)
                .OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Ngay).ToList();
            ViewBag.IsCurrent = isCurrent;
            return View(model);
        }
        public IActionResult AnnouncementDetail(int id)
        {
            var model = _context.Thong_Baos.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == id);
            ViewBag.Tournament = model.DS_Giai.Ten;
            return View(model);
        }
        public IActionResult Result(bool isCurrent, int id)
        {
            List<DS_Trinh> model = new();
            if (!isCurrent)
            {
                model = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.ID_Giai == id).OrderBy(m => m.Trinh).ToList();
                ViewBag.Tournament = model.FirstOrDefault().DS_Giai.Ten;

            }
            else
            {
                model = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi).OrderBy(m => m.Trinh).ToList();
                ViewBag.Tournament = model.FirstOrDefault().DS_Giai.Ten;
            }
            return View(model);
        }
        public IActionResult Tournament()
        {
            var model = _context.DS_Giais.Where(m => !m.Giai_Moi).OrderByDescending(m => m.Ngay).ToList();
            return View(model);
        }
        public IActionResult CheckPassword(int id, string action, string controller, string currentAction, string currentController)
        {
            var model = _context.DS_VDVs.Find(id);
            ViewBag.Password = model.Password;
            return PartialView(new PasswordViewModel
            {
                Id = model.Id,
                Ten_Tat = model.Ten_Tat,
                Action = action,
                Controller = controller,
                CurrentAction = currentAction,
                CurrentController = currentController
            });
        }
        [HttpPost]
        public IActionResult CheckPassword(int id, PasswordViewModel model)
        {
            var old = _context.DS_VDVs.Find(id);
            if (model.Password == old.Password/* && model.ConfirmPassword == model.NewPassword*/)
            {
                // First time user
                if (old.Password == "bitkhanhhoa@newuser")
                {
                    old.Password = model.NewPassword;
                    bool result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(old.Id, old, new List<string> { "Password" }).Succeeded;
                    if (result) _context.SaveChanges();

                }
                return RedirectToAction(model.Action, model.Controller, new { id });
            }
            else // Wrong password
            {
                _notyf.Error("Nhập sai Password!");
            }
            return RedirectToAction(model.CurrentAction, model.CurrentController);
        }
    }
}