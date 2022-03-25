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

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class CurrentTabController : Controller
    {
        private readonly TennisContext _context;
        public CurrentTabController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Guideline()
        {
            var current = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi);
            var model = _context.Thong_Baos.Where(m => m.ID_Giai == current.Id).ToList();
            ViewBag.Tournament = current.Ten;
            return View(model);
        }
        public IActionResult Pair()
        {
            var current = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi);
            var model = _context.DS_Caps.Include(m => m.DS_Trinh).Include(m => m.VDV1).Include(m => m.VDV2)
                .Where(m => m.DS_Trinh.ID_Giai == current.Id)
                .OrderBy(m => m.DS_Trinh.Trinh).ThenBy(m => m.Ma_Cap).ToList();
            var list = model.GroupBy(m => m.DS_Trinh.Trinh).Select(m => new
            {
                Trinh = m.Key,
                Num = m.Count()
            }).OrderBy(m => m.Trinh);
            ViewBag.ListLevel = list.Select(m => m.Trinh).ToList();
            ViewBag.ListNum = list.Select(m => m.Num).ToList();
            // Generate List of all participated players with no pairs
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == current.Id).Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            // Get all players with from Player Id found in Player1 and Player2 lists
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).ToList();
            ViewBag.Tournament = current.Ten;
            return View(model);
        }
        public IActionResult Register(int id)
        {
            var model = _context.DS_VDVs.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult Register(DS_VDV model, int id)
        {
            var old = _context.DS_VDVs.Find(id);
            //if (old.Email )
            TempData["SuccessfulRegister"] = true;
            return RedirectToAction("Player", "NoRole", new { isCurrent = true, participate = true });
            }
        public IActionResult ResultInfo(ResultViewModel model)
        {
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int id)
        {
            var level = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == id).FirstOrDefault();
            var vm = new ResultViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                Level = level.Trinh.ToString(),
                Tournament = level.DS_Giai.Ten
            };
            switch (tabname)
            {
                case "Table":
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
                case "Special":
                    vm.ActiveTab = Tab.Special;
                    return RedirectToAction("ResultInfo", vm);
                case "Point":
                    vm.ActiveTab = Tab.Point;
                    return RedirectToAction("ResultInfo", vm);
                default:
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
            }
        }
    }
}
