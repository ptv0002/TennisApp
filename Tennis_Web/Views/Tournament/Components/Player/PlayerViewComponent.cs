using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Player
{
    public class PlayerViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public PlayerViewComponent(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            var players = new List<DS_VDV>();
            if (vm.IsCurrent == true)
            {
                if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công!");
                // Display for current tournament
                players = _context.DS_VDVs.ToList();
            }
            else
            {
                // Display for previous tournament
                // Get all levels from given tournament
                var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == vm.ID).Select(m => m.Id).ToList();
                // Get all pairs with Level Id from the level id list
                var vdv1_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1).ToList();
                var vdv2_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2).ToList();
                // Get all players with from Player Id found in Player1 and Player2 lists
                players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id)).ToList();

            }
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(players);
        }
    }
}
