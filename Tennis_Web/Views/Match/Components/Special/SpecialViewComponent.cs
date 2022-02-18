using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            //var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == id);
            //var model = _context.DS_Trans.Include(m => m.DS_Cap1).Include(m => m.DS_Vong)
            //    .Where(m => pairs.Select(m => m.Id).Contains((int)m.ID_Cap1) || pairs.Select(m => m.Id).Contains((int)m.ID_Cap2))
            //    .OrderByDescending(m => m.DS_Vong.Ma_Vong)
            //    .ThenBy(m => m.DS_Cap1.Ma_Cap).ToList();
            //if (model != null)
            //{
            //    for (int i = 0; i < model.Count; i++)
            //    {
            //        var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap1).FirstOrDefault();
            //        var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap2).FirstOrDefault();
            //        // Assign Pair 1 and 2 info to list of matches
            //        model[i].DS_Cap1 = cap1;
            //        model[i].DS_Cap2 = cap2;
            //    }
            //}
            //var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == id).FirstOrDefault();
            //ViewBag.ListTable = pairs.Include(m => m.DS_Bang).GroupBy(m => m.DS_Bang.Ten).Select(m => new {
            //    Table = m.Key,
            //    Num = m.Count()
            //}).OrderBy(m => m.Table).ToList();
            return View(/*model*/);
        }
    }
}
