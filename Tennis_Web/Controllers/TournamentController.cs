using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly TennisContext _context;
        public TournamentController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index(bool? isCurrent)
        {
            var model = new List<DS_Trinh>();
            if (isCurrent == true)
            {
                ViewBag.isCurrent = true;
            }
            else model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool? isCurrent, int? TrinhID)
        {
            var vm = new TournamentTabViewModel()
            {
                IsCurrent = isCurrent,
                TrinhID = TrinhID
            };
            switch (tabname)
            {
                case "Parameter":
                    vm.ActiveTab = Tab.Parameter;
                    break;
                case "Player":
                    vm.ActiveTab = Tab.Player;
                    break;
                case "Division":
                    vm.ActiveTab = Tab.Division;
                    break;
                default:
                    vm.ActiveTab = Tab.Parameter;
                    break;
            }
            return RedirectToAction(nameof(TournamentInfo), vm);
        }
        public IActionResult TournamentInfo(TournamentTabViewModel model, bool? isCurrent, int? TrinhID)
        {

            if (model == null)
            {
                model = new TournamentTabViewModel
                {
                    ActiveTab = Tab.Parameter,
                    IsCurrent = isCurrent,
                    TrinhID = TrinhID
                };
            }
            return View(model);
        }
    }
}
