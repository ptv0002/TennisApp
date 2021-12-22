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
        private readonly IHostingEnvironment _environment;
        public TournamentController(TennisContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index(bool? isCurrent)
        {
            var model = new List<DS_Trinh>();
            if (isCurrent == true)
            {
                var path = Path.Combine(this._environment.WebRootPath, "Files/") + "Giải Đấu.xlsx";
                FileInfo file = new(path);
                using (var excelPack = new ExcelPackage(file))
                {
                    // Get Tournament name from Tournament sheet (DS_Giai) | index = 3 (start at 0)
                    var tournament = excelPack.Workbook.Worksheets[3];
                    DS_Giai giai = new()
                    {
                        Ten = tournament.Cells[2, 3].Text
                    };
                    // Get Level name from Level sheet (DS_Trinh) | index = 5 
                    var level = excelPack.Workbook.Worksheets[5];
                    int rowCount = level.Dimension.End.Row;
                    for (int row = 2; row < rowCount + 1; row++)
                    {
                        DS_Trinh trinh = new()
                        {
                            DS_Giai = giai,
                            Id = Convert.ToInt32(level.Cells[row, 1].Text),
                            Trinh = Convert.ToInt32(level.Cells[row, 6].Text)
                        };
                        model.Add(trinh);
                    }
                    model.OrderByDescending(m => m.Trinh);
                }
                ViewBag.isCurrent = true;
            }
            else model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool? isCurrent, int? trinhID, string detailedTitle)
        {
            var vm = new TournamentTabViewModel()
            {
                IsCurrent = isCurrent,
                TrinhID = trinhID,
                DetailedTitle = detailedTitle
            };
            switch (tabname)
            {
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    break;
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    break;
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    break;
                case "Division":
                    vm.ActiveTab = Tab.Division;
                    break;
                default:
                    vm.ActiveTab = Tab.Info;
                    break;
            }
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
        public IActionResult TournamentInfo(TournamentTabViewModel model, bool? isCurrent, int? trinhID, string detailedTitle)
        {
            if (model == null)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Info,
                    IsCurrent = isCurrent,
                    TrinhID = trinhID,
                    DetailedTitle = detailedTitle
                };
            }
            return View(model);
        }
    }
}
