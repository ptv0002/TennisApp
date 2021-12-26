using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _environment;
        public TournamentController(TennisContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(bool? isCurrent)
        {
            var model = new List<DS_Giai>();
            if (isCurrent == true)
            {
                // Get Tournament name from Tournament sheet (DS_Giai) | index = 3 (start at 0)
                var tournament = new MethodController(_context, _environment).GetWorkSheet("DS_Giai");
                DS_Giai giai = new()
                {
                    Ten = tournament.Cells[2, 4].Text
                };
                model.Add(giai);
                ViewBag.isCurrent = true;
            }
            else model = _context.DS_Giais.OrderByDescending(m => m.Ngay).ToList();

            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool? isCurrent, int? id, string detailedTitle)
        {
            var vm = new TournamentTabViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                DetailedTitle = detailedTitle
            };
            switch (tabname)
            {
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    return RedirectToAction(nameof(LevelInfo), vm);
                case "Division":
                    vm.ActiveTab = Tab.Division;
                    return RedirectToAction(nameof(LevelInfo), vm);
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                default:
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
            }
        }
        public IActionResult TournamentInfo(TournamentTabViewModel model, bool? isCurrent, int? giaiID, int? trinhID)
        {
            if (model.ID == null && model.IsCurrent == null)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Info,
                    IsCurrent = isCurrent,
                    ID = giaiID
                };
            }
            var levels = new List<DS_Trinh>();
            if (model.IsCurrent == true)
            {
                // Get Tournament name from Tournament sheet (DS_Giai) | index = 3 (start at 0)
                var tournament = new MethodController(_context, _environment).GetWorkSheet("DS_Giai");
                ViewBag.TournamentTitle = tournament.Cells[2, 4].Text;
                // Get Level name from Level sheet(DS_Trinh) | index = 5
                var levelSheet = new MethodController(_context, _environment).GetWorkSheet("DS_Trinh");
                int rowCount = levelSheet.Dimension.End.Row;
                for (int row = 2; row < rowCount + 1; row++)
                {
                    DS_Trinh item = new()
                    {
                        Id = Convert.ToInt32(levelSheet.Cells[row, 1].Text),
                        Trinh = Convert.ToInt32(levelSheet.Cells[row, 12].Text)
                    };
                    levels.Add(item);
                }
            }
            else
            {
                if (giaiID == null)
                {
                    var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == trinhID);
                    giaiID = temp.First().DS_Giai.Id;
                }
                levels = _context.DS_Trinhs.Where(m => m.DS_Giai.Id == giaiID).ToList();
            }
            ViewBag.LevelList = levels.OrderByDescending(m => m.Trinh);
            return View(model);
        }
        public IActionResult LevelInfo(TournamentTabViewModel model, bool? isCurrent, int? trinhID, string detailedTitle)
        {
            if (model.ID == null && model.IsCurrent == null)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Parameter,
                    IsCurrent = isCurrent,
                    ID = trinhID,
                    DetailedTitle = detailedTitle
                };
            }
            return View(model);
        }
        public IActionResult DeleteTournament()
        {
            var excel = new MethodController(_context, _environment).GetExcel();
            for (int i = 0; i < excel.Worksheets.Count; i++)
            {
                var sheet = new MethodController(_context, _environment).GetWorkSheet(i);
                sheet.Cells["2:1000"].Clear();
            }
            return RedirectToAction(nameof(Index), true); 
        }
        public IActionResult EndTournament()
        {
            var temp = new MethodController(_context, _environment);
            //temp.ImportFromExcel();
            return DeleteTournament();
        }
    }
}
