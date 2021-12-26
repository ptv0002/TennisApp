using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
            switch (a1, a2) 
            {
                case (true, false or true): // Current tournament
                    var sheet = new MethodController(_context, _environment).GetWorkSheet("DS_Giai");
                    item.Ngay = DateTime.TryParse(sheet.Cells[2, 3].Text, out var a) ? a : null;
                    item.Ten = sheet.Cells[2, 4].Text;
                    item.GhiChu = sheet.Cells[2, 2].Text;
                    break;
                case (false, false): // Previous tournament
                    var table = await _context.DS_Giais.FindAsync(vm.ID);
                    item.Ngay = table.Ngay;
                    item.Ten = table.Ten;
                    item.GhiChu = table.GhiChu;
                    break;
                default: // Error or other unanticipated scenario
                    ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
                    break;
            }
            return View(item);
        }
    }
}
