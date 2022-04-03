using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public readonly INotyfService _notyf;
        public CurrentTabController(TennisContext context,INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public List<DS_Cap> PopulateAutoComplete(int idVdv)
        {
            var levels = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi);
            // Get all pairs with Level Id from the level id list
            //var vdv_Ids = _context.DS_Caps.Where(m => levels.Select(m => m.Id).Contains(m.ID_Trinh)).SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2});
            //// Get all players with from Player Id found in Player1 and Player2 lists
            //var players = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id));
            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Select(m => m.Id).Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Select(m => m.Id).Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            var eligible = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).ToList();
            var model = new List<DS_Cap>();
            var info = _context.DS_VDVs.Find(idVdv);
            foreach (var partner in eligible)
            {
                var eligibleLevels = levels.Where(m => Math.Abs(m.Trinh - (info.Diem + partner.Diem)) <= 20).ToList();
                if (eligibleLevels.Any())
                {
                    foreach (var level in eligibleLevels)
                    {
                        model.Add(new DS_Cap
                        {
                            Diem = info.Diem + partner.Diem,
                            ID_Vdv2 = partner.Id,
                            VDV2 = partner,
                            ID_Trinh = level.Id
                        });
                    }                    
                }
            }
            return model;
        }
        public IActionResult UpdatePair(int idVdv, int idPair)
        {
            var model = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).FirstOrDefault(m => m.Id == idPair);

            if (model == null)
            {
                var levels = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi).OrderBy(m => m.Trinh);
                ViewBag.DS_Trinh = new SelectList(levels, "Id", "Trinh");
                ViewBag.LevelList = levels;
                ViewBag.DS_VDV = PopulateAutoComplete(idVdv);
                model = new DS_Cap { ID_Vdv1 = idVdv, VDV1 = _context.DS_VDVs.Find(idVdv) };
                model.VDV1.Password = null;
            }
            else model.VDV2.Password = null;
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult UpdatePair(DS_Cap pair)
        {
            var result = false;
            // New pair register
            if (pair.Id == 0)
            {
                var p1 = _context.DS_VDVs.Find(pair.ID_Vdv1);
                if (p1.Password == pair.VDV1.Password)
                {
                    pair.Diem = p1.Diem + _context.DS_VDVs.Find(pair.ID_Vdv2).Diem;
                    result = true;
                    _context.Add(pair);
                }
            }
            // Confirm part form
            else
            {
                if (_context.DS_VDVs.Find(pair.ID_Vdv2).Password == pair.VDV2.Password)
                {
                    result = true;
                    pair.Xac_Nhan = true;
                    pair.Phe_Duyet = true;
                    result &= new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xac_Nhan", "Phe_Duyet" }).Succeeded;
                }
            }
            if (result)
            {
                _context.SaveChanges();
                TempData["SuccessfulPair"] = true;
            }
            
            return RedirectToAction(nameof(Pair));
        }
        
        public IActionResult Pair()
        {
            bool? success = (bool?)TempData["SuccessfulPair"];
            if (success == true) { _notyf.Success("Lưu thay đổi thành công!"); }
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
            //var vdv_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2 });
            //// Get all players with from Player Id found in Player1 and Player2 lists
            //var players = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id));
            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).ToList();
            ViewBag.Tournament = current.Ten;
            return View(model);
        }
        public IActionResult Register(int id)
        {
            var model = _context.DS_VDVs.Find(id);
            ViewBag.Password = model.Password;
            return PartialView(new RegisterViewModel
            {
                Id = model.Id,
                Email = model.Email,
                Ten_Tat = model.Ten_Tat
            });
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model, int id)
        {
            var old = _context.DS_VDVs.Find(id);
            bool result = false;
            // First time user
            if (old.Password == "bitkhanhhoa@newuser" && model.Password == old.Password && model.ConfirmPassword == model.Password)
            {
                old.Email = model.Email;
                old.Password = model.NewPassword;
                old.Tel = model.Tel;
                old.Phe_Duyet = true;
                result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(old.Id, old, new List<string> { "Email", "Tel", "Password", "Phe_Duyet" }).Succeeded;
            }
            else
            {
                if (old.Password == model.Password)
                {
                    old.Phe_Duyet = true;
                    result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(old.Id, old, new List<string> { "Phe_Duyet" }).Succeeded;
                }
            }
            TempData["SuccessfulRegister"] = result;
            if (result) { _context.SaveChanges(); }
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
