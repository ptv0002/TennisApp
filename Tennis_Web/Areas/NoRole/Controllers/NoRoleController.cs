using DataAccess;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;
using Library;
using Models;
using Tennis_Web.Models;
using Tennis_Web.Areas.NoRole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class NoRoleController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public NoRoleController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public async Task<IActionResult> Index()
        {
            //var temp = new ExcelMethod<DS_Vong>();
            //ResultModel<DS_Vong> a = temp.ExcelToList("C:/TennisApp/Excel/Giai_Dau.xlsx", "DS_Vong");
            //_context.AddRange(a.List);
            //_context.SaveChanges();
            return View();
        }
        public IActionResult Player()
        {
            var model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }

        public IActionResult Result()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult ResultInfo(ResultViewModel model)
        {
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int id)
        {
            var level = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == id).FirstOrDefault();
            var vm = new ResultViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                Level = level.Trinh.ToString(),
                Tournament = level.DS_Giai.Ten
            };
            switch (tabname)
            {
                case "Table":
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
                case "Special":
                    vm.ActiveTab = Tab.Special;
                    return RedirectToAction("ResultInfo", vm);
                default:
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
            }
        }
        public IActionResult Announcement()
        {
            return View();
        }
        
    }
}