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

            // Generate List of all participated players with no pairs
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == _context.DS_Trinhs.Find(vm.ID).ID_Giai).Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            var vdv_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2 });
            // Get all players with from Player Id found in Player1 and Player2 lists
            var players = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id));
            ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).ToList();
            return View(model);
        }
    }
}
