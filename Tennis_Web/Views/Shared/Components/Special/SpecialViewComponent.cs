using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library.FileInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Special
{
    public class SpecialViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        private readonly IWebHostEnvironment _webHost;
        public SpecialViewComponent(TennisContext context, INotyfService notyf, IWebHostEnvironment webHost)
        {
            _context = context;
            _notyf = notyf;
            _webHost = webHost;
        }
        public async Task<IViewComponentResult> InvokeAsync(TabViewModel vm, ResultViewModel alt)
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

            var matches = _context.DS_Trans.Include(m => m.DS_Cap1.VDV1).Include(m => m.DS_Cap1.VDV2)
                .Include(m => m.DS_Cap2.VDV1).Include(m => m.DS_Cap2.VDV2)
                .Where(m => m.ID_Trinh == vm.ID && m.Ma_Vong <= 6) // Ma_Vong <= 6 are special rounds
                .ToList().OrderBy(m => m.Ma_Tran[^3..]).ToList();
            ViewBag.IsCurrent = vm.IsCurrent;
            if (matches.Any()) 
            {
                var pairIds = matches.SelectMany(m => new[] { m.ID_Cap1, m.ID_Cap2 });
                var pairs = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => pairIds.Contains(m.Id)).Select(m => new { 
                    m.Id,
                    PairName = m.VDV1.Ten_Tat + " + " + m.VDV2.Ten_Tat
                });
                ViewBag.PairIds = new SelectList(pairs, "Id", "PairName");  
                ViewBag.RoundNum = matches.Max(m => m.Ma_Vong);
            }
            return View(matches);
        }
    }
}
