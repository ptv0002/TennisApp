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

namespace Tennis_Web.Views.Shared.Components.Point
{
    public class PointViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public PointViewComponent(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
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
            FileStream fileStream = File.OpenRead(_webHost.WebRootPath + "/Files/Json/RoundInfo.json");
            ViewBag.ListRound = (await JsonSerializer.DeserializeAsync<List<Round>>(fileStream)).ToDictionary(x => x.Ma_Vong, y =>
            {
                var name = y.Ten.Split(" ");
                return name[0].ToString() == "Vòng" ? 'V' + name[1] : name[0][0].ToString() + name[1][0].ToString();
            });
            fileStream.Dispose();

            var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID);
            var list = pairs.Include(m => m.DS_Bang).GroupBy(m => m.DS_Bang.Ten).Select(m => new
            {
                Table = m.Key,
                Num = m.Count()
            }).OrderBy(m => m.Table);
            ViewBag.ListTable = list.Select(m => m.Table).ToList();
            ViewBag.ListNum = list.Select(m => m.Num).ToList();
            ViewBag.IsCurrent = vm.IsCurrent;
            ViewBag.RoundNum = _context.DS_Trans.Where(m => m.ID_Trinh == vm.ID && m.Ma_Vong <= 6).Max(m => m.Ma_Vong);// Ma_Vong <= 6 are special rounds
            ViewBag.Playoff = _context.DS_Trans.Any(m => m.ID_Trinh == vm.ID && m.Ma_Vong == 7);
            var model = _context.DS_Caps.Where(m => m.ID_Trinh == vm.ID).Include(m => m.VDV1).Include(m => m.VDV2).OrderBy(m => m.Ma_Cap).ToList();
            return View(model);
        }
    }
}
