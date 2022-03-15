using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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
        public class ConfirmViewModal
        {
            public string BtnMsg { get; set; }
            public string Message { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public string Id { get; set; }
        }
        
        public IActionResult ConfirmModal(ConfirmViewModal model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ConfirmModal(ConfirmViewModal model, string any = "")
        {
            return RedirectToAction(model.ActionName, model.ControllerName, new { id = model.Id });
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
    }
}
