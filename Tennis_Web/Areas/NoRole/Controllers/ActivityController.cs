using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class ActivityController : Controller
    {
        private readonly TennisContext _context;
        public ActivityController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult ActivityIndex(int tab)
        {
            var activityMenu = Library.FileInitializer.Initializer.ActivityMenu();
            List<Media> model = new();
            if (tab != 0)
            {
                model = _context.Medias.Include(m => m.DS_Giai).Where(m => m.Ma_Menu == tab)
                    .OrderByDescending(m => m.DS_Giai.Ngay).ToList();
            }
            ViewBag.Title = activityMenu[tab];
            return View(model);
        }
        public IActionResult ActivityDetail(int id)
        {
            var model = _context.Medias.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == id);
            var activityMenu = Library.FileInitializer.Initializer.ActivityMenu();
            ViewBag.Title = activityMenu[model.Ma_Menu];
            return View(model);
        }
    }
}
