using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Controllers;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Player
{
    public class PlayerViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly IWebHostEnvironment _environment;
        public PlayerViewComponent(TennisContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            bool a1 = vm.IsCurrent == true;
            bool a2 = vm.ID == null;
            var model = new List<DS_VDV>();
            //switch (a1, a2)
            //{
            //    case (true, false): // Current tournament
            //        var player = new MethodController(_context, _environment).GetWorkSheet("DS_VDV");
            //        int rowCount = player.Dimension.End.Row;
            //        for (int row = 2; row < rowCount + 1; row++) // Row index starts at 1
            //        {
            //            DS_VDV item = new()
            //            {
            //                Ten_Tat = player.Cells[row, 15].Text,
            //                CLB = player.Cells[row, 2].Text,
            //                DiemCu = Convert.ToInt32(player.Cells[row, 6].Text),
            //                Diem = Convert.ToInt32(player.Cells[row, 5].Text)
            //            };
            //            model.Add(item);
            //        }
            //        model.OrderByDescending(m => m.DiemCu);
            //        break;
            //    case (false, false): // Previous tournament
            //        var dsCap = await _context.DS_Caps./*Include(m => m.DS_Trinh).*/Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.DS_Trinh.Id == vm.ID).ToListAsync();
            //        foreach (var cap in dsCap)
            //        {
            //            model.Add(cap.VDV1);
            //            model.Add(cap.VDV2);
            //            //if (cap.DS_Trinh.Id == vm.TrinhID)
            //            //{
            //            //    model.Add(cap.VDV1);
            //            //    model.Add(cap.VDV2);
            //            //}
            //        }
            //        model.OrderByDescending(m => m.Diem);
            //        break;
            //    default: // New tournament, error or other unanticipated scenario
            //        break;
            //}
            return View(model);
        }
    }
}
