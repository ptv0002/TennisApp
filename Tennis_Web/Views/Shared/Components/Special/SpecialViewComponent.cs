using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library.FileInitializer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

namespace Tennis_Web.Views.Match.Components.Special
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
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");

            List<DS_Tran> matches = new();
            if (vm.Succeeded != false)
            {
                matches = matches = _context.DS_Trans
                    .Where(m => m.ID_Trinh == vm.ID && m.Ma_Vong <= 6) // Ma_Vong <= 6 are special rounds
                    .ToList().OrderBy(m => m.Ma_Tran[^3..]).ToList();
            }
            FileStream fileStream = File.OpenRead(_webHost.WebRootPath + "/Files/Json/RoundInfo.json");
            ViewBag.ListRound = (await JsonSerializer.DeserializeAsync<List<Round>>(fileStream)).ToDictionary(x => x.Ma_Vong, y => y.Ten);
            fileStream.Dispose();
            ViewBag.IsCurrent = vm.IsCurrent;
            if (matches.Any()) ViewBag.RoundNum = matches.Max(m => m.Ma_Vong);
            
            var model = new RoundTabViewModel
            {
                ID_Trinh = vm.ID,
                DS_Tran = matches,
                DS_Cap = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID).Include(m => m.VDV1).Include(m => m.VDV2).OrderBy(m => m.Ma_Cap).ToList()
            };
            return View(model);
        }
    }
}
