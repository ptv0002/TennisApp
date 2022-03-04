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
            //FileInfo file = new("D:/Giai_Dau.xlsx");
            //var temp = new SetExcelMethod<DS_Giai>();
            //temp.ImportWorkSheet("D:/Giai_Dau.xlsx", "DS_Giai");
            // Get path for the Json file
            //string path = _webHost.WebRootPath + "/Files/Json/RoundInfo.json";
            //var listRound = new List<Vong>
            //            {
            //                new Vong { Ten = "Vô Địch", Ma_Vong = 0 },
            //                new Vong { Ten = "Chung Kết", Ma_Vong = 1 },
            //                new Vong { Ten = "Bán Kết", Ma_Vong = 2 },
            //                new Vong { Ten = "Tứ Kết", Ma_Vong = 3 },
            //                new Vong { Ten = "Vòng 3", Ma_Vong = 4 },
            //                new Vong { Ten = "Vòng 2", Ma_Vong = 5 },
            //                new Vong { Ten = "Vòng 1", Ma_Vong = 6 },
            //                new Vong { Ten = "Playoff", Ma_Vong = 7 },
            //                new Vong { Ten = "Vòng Bảng", Ma_Vong = 8 }
            //            };
            //FileStream fileStream;
            //// Delete file if Json file is already exist
            //if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            //fileStream = System.IO.File.Create(path);
            //var options = new JsonSerializerOptions { WriteIndented = true };
            //await JsonSerializer.SerializeAsync(fileStream, listRound, options);
            //fileStream.Dispose();

            //FileStream roundStream = System.IO.File.OpenRead(path);
            //var roundFile = (await JsonSerializer.DeserializeAsync<List<Vong>>(roundStream)).ToDictionary(x => x.Ma_Vong, y => y.Ten);
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