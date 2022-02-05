using AspNetCoreHero.ToastNotification.Abstractions;
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
        public readonly INotyfService _notyf;
        public LevelListViewComponent(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            ViewBag.IsCurrent = vm.IsCurrent;
            ViewBag.ID_Giai = vm.ID;
            if (vm.Succeeded == true) _notyf.Success("Xóa trình thành công!"); 
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.ID_Giai == vm.ID).OrderBy(m => m.Trinh).ToList();
            return View(model);
        }
    }
}
