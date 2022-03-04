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

namespace Tennis_Web.Views.Match.Components.Table
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
            }
            List<DS_Tran> matches = new();
            //if (vm.CurrentModel != null) model = vm.CurrentModel.Cast<DS_Tran>().ToList();
            
            var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID);
            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công");
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");
            
            if (vm.Succeeded != false)
            {
                matches = _context.DS_Trans.Include(m => m.DS_Cap1).Include(m => m.DS_Cap2).Include(m => m.DS_Vong)
                    .Where(m => pairs.Select(m => m.Id).Contains((int)m.ID_Cap1) || pairs.Select(m => m.Id).Contains((int)m.ID_Cap2)
                    && m.DS_Vong.Ma_Vong > 6) // Ma_Vong > 6 are Table and Playoff rounds
                    .ToList().OrderBy(m => m.Ma_Tran[^3..]).ToList();
                if (matches.Any(m => m.DS_Vong.Ma_Vong == 7)) // Check to see if all Table rounds are filled
                    ViewBag.Ready = matches.Where(m => m.DS_Vong.Ma_Vong == 8).All(m => (m.Kq_1 + m.Kq_2) > 0);
            }
            var list = pairs.Include(m => m.DS_Bang).GroupBy(m => m.DS_Bang.Ten).Select(m => new
            {
                Table = m.Key,
                Num = m.Count()
            }).OrderBy(m => m.Table);
            ViewBag.ListTable = list.Select(m => m.Table).ToList();
            ViewBag.ListNum = list.Select(m => m.Num).ToList();
            var model = new TableTabViewModel
            {
                DS_Tran = matches,
                //DS_Cap = pairs.Include(m => m.VDV1).Include(m => m.VDV2).OrderBy(m => m.Id).ToList()
                DS_Cap = pairs.Include(m => m.VDV1).Include(m => m.VDV2).OrderBy(m => m.Ma_Cap).ToList()
            };
            return View(model);
        }
    }
}
