using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.LevelList
{
    public class LevelListViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public LevelListViewComponent(TennisContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            ViewBag.IsCurrent = vm.IsCurrent;
            var model = await _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.ID_Giai == vm.ID).ToListAsync();
            return View(model);
        }
    }
}
