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

namespace Tennis_Web.Views.Shared.Components.Parameter
{
    public class ParameterViewComponent : ViewComponent 
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _environment;
        public ParameterViewComponent(TennisContext context, IWebHostEnvironment environment) 
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            bool a1 = vm.IsCurrent == true;
            bool a2 = vm.ID == null;
            DS_Trinh item = new();
            if (vm.ID != null)
            {
                var table = await _context.DS_Trinhs.FindAsync(vm.ID);
                item.Id = table.Id;
                item.Trinh = table.Trinh;
                item.TL_VoDich = table.TL_VoDich;
                item.TL_ChungKet = table.TL_ChungKet;
                item.TL_BanKet = table.TL_BanKet;
                item.TL_TuKet = table.TL_TuKet;
                item.TL_Bang = table.TL_Bang;
                item.TongDiem = table.TongDiem;
                item.ChenhLech = table.ChenhLech;
                item.Diem_PB = table.Diem_PB;
                item.DiemTru = table.DiemTru;
            }
            else
            {
                    // Error or other unanticipated scenario
                    ModelState.AddModelError(string.Empty, "Lỗi hệ thống!"); 
            }
            //switch (a1, a2)
            //{
            //    case (true, false or true): // Current tournament
            //        var temp = new MethodController(_context, _environment);
            //        var sheet = temp.GetWorkSheet("DS_Trinh");
            //        var row = (int)vm.ID;
            //        item.Id = Convert.ToInt32(sheet.Cells[row, temp.GetColumn("Id", sheet)].Text);
            //        item.ChenhLech = int.TryParse(sheet.Cells[row, temp.GetColumn("ChenhLech", sheet)].Text, out var a) ? a : null;
            //        item.DiemTru = int.TryParse(sheet.Cells[row, temp.GetColumn("DiemTru", sheet)].Text, out var b) ? b : null;
            //        item.Diem_PB = int.TryParse(sheet.Cells[row, temp.GetColumn("Diem_PB", sheet)].Text, out var c) ? c : null;
            //        item.TL_BanKet = decimal.TryParse(sheet.Cells[row, temp.GetColumn("TL_BanKet", sheet)].Text, out var d) ? d : null;
            //        item.TL_Bang = decimal.TryParse(sheet.Cells[row, temp.GetColumn("TL_Bang", sheet)].Text, out var e) ? e : null;
            //        item.TL_ChungKet = decimal.TryParse(sheet.Cells[row, temp.GetColumn("TL_ChungKet", sheet)].Text, out var f) ? f : null;
            //        item.TL_TuKet = decimal.TryParse(sheet.Cells[row, temp.GetColumn("TL_TuKet", sheet)].Text, out var g) ? g : null;
            //        item.TL_VoDich = decimal.TryParse(sheet.Cells[row, temp.GetColumn("TL_VoDich", sheet)].Text, out var h) ? h : null;
            //        item.TongDiem = int.TryParse(sheet.Cells[row, temp.GetColumn("TongDiem", sheet)].Text, out var i) ? i : null;
            //        item.Trinh = int.TryParse(sheet.Cells[row, temp.GetColumn("Trinh", sheet)].Text, out var j) ? j : null;
            //        break;
            //    case (false, false): // Previous tournament
                    
            //        break;
            //    default: 
            //        break;
            //}
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(item);
        }
    }
}
