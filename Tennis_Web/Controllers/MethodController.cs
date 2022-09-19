using DataAccess;
using Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class MethodController : Controller
    {
        private readonly TennisContext _context;
        public MethodController(TennisContext context)
        {
            _context = context;
        }
        public class ConfirmViewModal
        {
            public string BtnMsg { get; set; }
            public string Message { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public string AreaName { get; set; }
            public string Id { get; set; }
        }

        public IActionResult ConfirmModal(ConfirmViewModal model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ConfirmModal(ConfirmViewModal model, string any = "")
        {
            if (model.ControllerName == null)
            {
                return RedirectToAction(model.ActionName, new { area = model.AreaName, id = model.Id });
            }
            return RedirectToAction(model.ActionName, model.ControllerName, new { area = model.AreaName, id = model.Id });
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int id, string detailedTitle)
        {
            var vm = new TabViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                DetailedTitle = detailedTitle
            };
            switch (tabname)
            {
                case "Table":
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("MatchInfo", "Match", vm);
                case "Special":
                    vm.ActiveTab = Tab.Special;
                    return RedirectToAction("MatchInfo", "Match", vm);
                case "Point":
                    vm.ActiveTab = Tab.Point;
                    return RedirectToAction("MatchInfo", "Match", vm);
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    return RedirectToAction("LevelInfo", "Level", vm);
                case "Pair":
                    vm.ActiveTab = Tab.Pair;
                    return RedirectToAction("LevelInfo", "Level", vm);
                case "Info":
                    vm.ActiveTab = Tab.Info;
                    return RedirectToAction("TournamentInfo", "Tournament", vm);
                case "LevelList":
                    vm.ActiveTab = Tab.LevelList;
                    return RedirectToAction("TournamentInfo", "Tournament", vm);
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    return RedirectToAction("TournamentInfo", "Tournament", vm);
                default:
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("MatchInfo", "Match", vm);
            }
        }
        public IActionResult TabVMGenerator_Level(int idTrinh, bool? result, Tab tabName, string msg,
            string action, string controller, bool editable)
        {
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == idTrinh);
            var vm = new TabViewModel
            {
                ActiveTab = tabName,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
                Succeeded = result,
                Editable = editable,
                ErrorMsg = msg
            };
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction(action, controller, vm);
        }
        public IActionResult TabVMGenerator_Tournament(int idGiai, bool result, Tab tabName)
        {
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = tabName,
                IsCurrent = true,
                ID = idGiai,
                Succeeded = result
            };
            return RedirectToAction("TournamentInfo","Tournament", vm);
        }

        public void DeletePair_Method(DS_Cap pair)
        {
            var matches = _context.DS_Trans.Where(m => m.ID_Cap1 == pair.Id || m.ID_Cap2 == pair.Id);
            _context.RemoveRange(matches);
            _context.Remove(pair);
            _context.SaveChanges();
            new ScoreCalculation(_context).Point_Deposit(pair.ID_Trinh);
        }
    }
}
