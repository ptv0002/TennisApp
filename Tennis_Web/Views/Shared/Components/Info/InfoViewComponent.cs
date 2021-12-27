using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Controllers;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Info
{
    public class InfoViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _environment;
        public InfoViewComponent(TennisContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            bool a1 = vm.IsCurrent == true;
            bool a2 = vm.ID == null;
            DS_Giai item = new();
            var levels = new List<DS_Trinh>();
            switch (a1, a2) 
            {
                case (true, false or true): // Current tournament
                    var temp = new MethodController(_context, _environment);
                    var tourSheet = temp.GetWorkSheet("DS_Giai");
                    item.Ngay = DateTime.TryParse(tourSheet.Cells[2, temp.GetColumn("Ngay", tourSheet)].Text, out var a) ? a : null;
                    item.Ten = tourSheet.Cells[2, temp.GetColumn("Ten", tourSheet)].Text;
                    item.GhiChu = tourSheet.Cells[2, temp.GetColumn("GhiChu", tourSheet)].Text;
                    levels = temp.GetLevelList();
                    break;
                case (false, false): // Previous tournament
                    var table = await _context.DS_Giais.FindAsync(vm.ID);
                    item.Ngay = table.Ngay;
                    item.Ten = table.Ten;
                    item.GhiChu = table.GhiChu;
                    levels = await _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Id == vm.ID).ToListAsync();
                    break;
                default: // Error or other unanticipated scenario
                    ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
                    break;
            }
            ViewBag.IsCurrent = vm.IsCurrent;
            ViewBag.LevelList = levels;
            return View(item);
        }
    }
}
