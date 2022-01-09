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
        public PlayerViewComponent(TennisContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            var players = new List<DS_VDV>();
            if (vm.IsCurrent == true) 
            {
                // Display for current tournament
            }
            else 
            {
                /*
                // Display for previous tournament
                // Get all levels from given tournament
                var levels = await _context.DS_Trinhs.Where(m => m.ID_Giai == vm.ID).Select(m => m.Id).ToListAsync();
                // Get all pairs with Level Id from the level id list
                var vdv1_Ids = await _context.DS_Caps.Where(m => levels.Contains((int)m.ID_Trinh)).Select(m => m.ID_Vdv1).ToListAsync();
                var vdv2_Ids = await _context.DS_Caps.Where(m => levels.Contains((int)m.ID_Trinh)).Select(m => m.ID_Vdv2).ToListAsync();
                // Get all players with from Player Id found in Player1 and Player2 lists
                players = await _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id)).ToListAsync();
                */
            }
            players = await _context.DS_VDVs.ToListAsync();
            
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(players);
        }
    }
}
