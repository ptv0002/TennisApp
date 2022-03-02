using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Match.Components.Special
{
    public class SpecialViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public SpecialViewComponent(TennisContext context, INotyfService notyf)
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
            List<DS_Tran> model = new();
            //if (vm.CurrentModel != null) model = vm.CurrentModel.Cast<DS_Tran>().ToList();

            var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID);
            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công");
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");
            
            var all = _context.DS_Trans.Include(m => m.DS_Cap1).Include(m => m.DS_Vong)
                    .Where(m => pairs.Select(m => m.Id).Contains((int)m.ID_Cap1) || pairs.Select(m => m.Id).Contains((int)m.ID_Cap2))
                    .OrderByDescending(m => m.DS_Vong.Ma_Vong)
                    .ThenBy(m => m.DS_Cap1.Ma_Cap).ToList();
            
            if (vm.Succeeded != false)
            {
                
                model = all.Where(m => m.DS_Vong.Ma_Vong <= 6).ToList(); // Ma_Vong <= 6 are special rounds  
                if (model != null)
                {
                    for (int i = 0; i < model.Count; i++)
                    {
                        var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap1).FirstOrDefault();
                        var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap2).FirstOrDefault();
                        // Assign Pair 1 and 2 info to list of matches
                        model[i].DS_Cap1 = cap1;
                        model[i].DS_Cap2 = cap2;
                    }
                }
                //var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == vm.ID).FirstOrDefault();
            }
            ViewBag.ListTable = pairs.Include(m => m.DS_Bang).GroupBy(m => m.DS_Bang.Ten).Select(m => new
            {
                Table = m.Key,
                Num = m.Count()
            }).OrderBy(m => m.Table).ToList();
            // Check to see if all previous rounds are filled
            ViewBag.Show = all.Where(m => m.DS_Vong.Ma_Vong > 6).All(m => (m.Kq_1 + m.Kq_2) > 0);
            return View(model);
        }
    }
}
