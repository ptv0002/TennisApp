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
                // Assign default value for first time access
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
                var temp = new MethodController(_context, _environment);
                // Get Tournament name from Tournament sheet (DS_Giai)
                var tourSheet = temp.GetWorkSheet("DS_Giai");
                ViewBag.TournamentTitle = tourSheet.Cells[2, temp.GetColumn("Ten", tourSheet)].Text;
                // Get Level name from Level sheet(DS_Trinh)
                levels = temp.GetLevelList();
            }
            else
            {
                // Handle going back to Tournament Info from Level Info
                if (giaiID == null)
                {
                    var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == trinhID);
                    giaiID = temp.First().DS_Giai.Id;
                }
                model.ID = giaiID;
                levels = _context.DS_Trinhs.Where(m => m.DS_Giai.Id == giaiID).ToList();
            }
            ViewBag.LevelList = levels.OrderByDescending(m => m.Trinh);
            return View(model);
        }
        public IActionResult LevelInfo(TournamentTabViewModel model, bool? isCurrent, int? trinhID, string detailedTitle)
        {
            if (model.ID == null && model.IsCurrent == null)
            {
                // Assign default value for first time access
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
        [HttpPost]
        public IActionResult UpdateInfo(DS_Giai item)
        {
           
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.Info,
                IsCurrent = true
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
        [HttpPost]
        public IActionResult UpdateParameter(DS_Trinh item)
        {
            var temp = new MethodController(_context, _environment);
            var excel = temp.GetExcel();
            var row = item.Id;
            using (excel)
            {
                var sheet = excel.Workbook.Worksheets["DS_Trinh"];
                sheet.Cells[row, temp.GetColumn("DiemTru", sheet)].Value = item.DiemTru;
                sheet.Cells[row, temp.GetColumn("Diem_PB", sheet)].Value = item.Diem_PB;
                sheet.Cells[row, temp.GetColumn("TL_BanKet", sheet)].Value = item.TL_BanKet;
                sheet.Cells[row, temp.GetColumn("TL_Bang", sheet)].Value = 100 - item.TL_VoDich - item.TL_ChungKet - item.TL_BanKet - item.TL_TuKet; // TL_Bang
                sheet.Cells[row, temp.GetColumn("TL_ChungKet", sheet)].Value = item.TL_ChungKet;
                sheet.Cells[row, temp.GetColumn("TL_TuKet", sheet)].Value = item.TL_TuKet;
                sheet.Cells[row, temp.GetColumn("TL_VoDich", sheet)].Value = item.TL_VoDich;
                excel.Save();
            }
            var tourSheet = temp.GetWorkSheet("DS_Giai");
            var levSheet = temp.GetWorkSheet("DS_Trinh");
            string tournament = tourSheet.Cells[2, temp.GetColumn("Ten", tourSheet)].Text;
            string level = levSheet.Cells[row, temp.GetColumn("Trinh", levSheet)].Text; 
            // Assign value for view model
            var vm = new TournamentTabViewModel
            {
                ActiveTab = Tab.Parameter,
                IsCurrent = true,
                ID = row,
                DetailedTitle = "Giải " + tournament + " - Trình " + level
            };
            return RedirectToAction(nameof(LevelInfo), vm);
        }
        public IActionResult DeleteTournament()
        {
            var excel = new MethodController(_context, _environment).GetExcel();
            using (excel)
            {
                for (int i = 0; i < excel.Workbook.Worksheets.Count; i++)
                {
                    // Clear cells
                    var sheet = excel.Workbook.Worksheets[i];
                    sheet.Cells["2:1000"].Clear();
                }
                excel.Save();
            }
            return RedirectToAction(nameof(Index), true); 
        }
        public IActionResult EndTournament()
        {
            // Import all data from Excel file
            var temp = new MethodController(_context, _environment);
            temp.ImportFromExcel(temp.GetExcel());
            // Clear Excel file after
            return DeleteTournament();
        }
    }
}
