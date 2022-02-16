using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Level.Components.Pair
{
    public class PairViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public PairViewComponent(TennisContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            ViewBag.IsCurrent = vm.IsCurrent;
            ViewBag.ID_Trinh = vm.ID;
            var model = _context.DS_Caps.Include(m => m.DS_Trinh).Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.ID_Trinh == vm.ID).OrderBy(m => m.Ma_Cap).ToList();
            return View(model);
        }
    }
}
