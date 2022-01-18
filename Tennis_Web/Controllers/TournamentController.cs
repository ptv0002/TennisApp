using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Library;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TennisContext _context;
        public TournamentController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index(bool isCurrent)
        {
            var model = _context.DS_Giais.OrderByDescending(m => m.Ngay).Where(m => m.GiaiMoi == isCurrent).ToList();
            ViewBag.isCurrent = isCurrent;
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int? id, string detailedTitle)
        {
            var vm = new TabViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                DetailedTitle = detailedTitle
            };
            switch (tabname)
            {
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    return RedirectToAction("LevelInfo", "Level", vm);
                case "Pair":
                    vm.ActiveTab = Tab.Pair;
                    return RedirectToAction("LevelInfo", "Level", vm);
                case "Division":
                    vm.ActiveTab = Tab.Division;
                    return RedirectToAction("LevelInfo", "Level", vm);
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                case "LevelList":
                    vm.ActiveTab = Tab.LevelList;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    return RedirectToAction(nameof(TournamentInfo), vm);
                default:
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction(nameof(TournamentInfo), vm);
            }
        }
        public IActionResult TournamentInfo(TabViewModel model, bool isCurrent, int? giaiID, int? trinhID)
        {
            bool a1 = giaiID == null;
            bool a2 = trinhID == null;
            bool a3 = model.ID == null;
            // Assign default value for first time access
            if (!a1 && a3)
            {
                model = new TabViewModel
                {
                    ActiveTab = Tab.Info,
                    IsCurrent = isCurrent,
                    ID = giaiID
                };
            }
            if (a1 && !a2)
            {
                var temp = _context.DS_Trinhs.Find(trinhID);
                model.ID = temp.ID_Giai;
            }
            else if (a1 && a2 && a3)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
                return View(model);
            }
            ViewBag.TournamentTitle = _context.DS_Giais.Find(model.ID).Ten;
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateInfo(DS_Giai item)
        {
            // Find and update Tournament Info
            var columnsToSave = new List<string> { "Ten", "GhiChu", "Ngay"};
            var result = new DatabaseMethod<DS_Giai>(_context).SaveObjectToDB(item.Id, item, columnsToSave);
            _context.SaveChanges();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.Info,
                IsCurrent = true,
                ID = item.Id,
                Succeeded = result.Succeeded
            };

            // If save unsuccessfully, view error and display View with "item" 
            if (!result.Succeeded) vm.CurrentModel = item;
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction(nameof(TournamentInfo), vm);

        }
        public IActionResult EndTournament(int id)
        {
            // Find the current Tournament and set IsCurrent to false
            var item = _context.DS_Giais.Find(id);
            item.GiaiMoi = false;
            _context.Update(item);
            // Reset Participation status to all false and Pair code to null
            var list = _context.DS_VDVs.ToList();
            list.ForEach(m =>
                            {
                                m.Tham_Gia = false;
                                m.Ma_Cap = null;
                            });
            _context.Update(list);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index), true);
        }
        public IActionResult AddLevel(string newLevel, string idGiai)
        {
            _context.Add(new DS_Trinh
            {
                Trinh = Convert.ToInt32(newLevel),
                ID_Giai = Convert.ToInt32(idGiai)
            });
            _context.SaveChanges();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.LevelList,
                IsCurrent = true,
                ID = Convert.ToInt32(idGiai)
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
        public IActionResult DeleteLevel(int id)
        {
            var item = _context.DS_Trinhs.Find(id);
            _context.Remove(item);
            _context.SaveChanges();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.LevelList,
                IsCurrent = true,
                ID = item.ID_Giai
            };
            return RedirectToAction(nameof(TournamentInfo), vm);
        }

    }
}
