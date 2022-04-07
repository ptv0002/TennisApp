using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Table
{
    public class TableViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public TableViewComponent(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IViewComponentResult Invoke(TabViewModel vm, ResultViewModel alt)
        {
            if (vm == null)
            {
                vm = new()
                {
                    ActiveTab = alt.ActiveTab,
                    IsCurrent = alt.IsCurrent,
                    ID = alt.ID
                };
                ViewBag.Admin = false;
            }
            else ViewBag.Admin = true;

            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công");
            else if (vm.Succeeded == false) _notyf.Error(vm.ErrorMsg ?? "Có lỗi xảy ra khi đang lưu thay đổi!", 30);

            var matches = _context.DS_Trans
                    .Where(m => m.ID_Trinh == vm.ID && m.Ma_Vong > 6) // Ma_Vong > 6 are Table and Playoff rounds
                    .ToList().OrderBy(m => m.Ma_Tran[^3..]).ToList(); 
            if (matches.Any())
            {
                //var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID && m.Phe_Duyet);
                var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID);
                var list = pairs.Include(m => m.DS_Bang).GroupBy(m => m.DS_Bang.Ten).Select(m => new
                {
                    Table = m.Key,
                    Num = m.Count()
                }).OrderBy(m => m.Table);
                ViewBag.ListTable = list.Select(m => m.Table).ToList();
                ViewBag.ListNum = list.Select(m => m.Num).ToList();
                ViewBag.IsCurrent = vm.IsCurrent;
                return View(new RoundTabViewModel
                {
                    ID_Trinh = vm.ID,
                    DS_Tran = matches,
                    DS_Cap = pairs.Include(m => m.VDV1).Include(m => m.VDV2).OrderBy(m => m.Ma_Cap).ToList()
                });
            }
            else
            {
                return View(new RoundTabViewModel());
            }
        }
    }
}
