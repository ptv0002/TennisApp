using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        public IActionResult Index(TournamentTabViewModel model)
        {
            if (model == null)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Parameters
                };
            }
            return View();
        }
        public IActionResult SwitchToTabs(string tabname )
        {
            var vm = new TournamentTabViewModel();
            switch (tabname)
            {
                case "Parameters":
                    vm.ActiveTab = Tab.Parameters;
                    break;
                case "Players":
                    vm.ActiveTab = Tab.Players;
                    break;
                case "Divisions":
                    vm.ActiveTab = Tab.Divisions;
                    break;
                default:
                    vm.ActiveTab = Tab.Parameters;
                    break;
            }
            return RedirectToAction(nameof(Index), vm);
        }
        public IActionResult ShowTournament(bool? isCurrent)
        {
            if (isCurrent == true)
            {
                
            }
            else
            {

            }
            return View();
        }
    }
}
