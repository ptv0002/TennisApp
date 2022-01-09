using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Shared.Components.Parameter
{
    public class ParameterViewComponent : ViewComponent 
    {
        private readonly TennisContext _context;
        public ParameterViewComponent(TennisContext context) 
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            var item = (DS_Trinh)vm.CurrentModel;
            if (item == null)
            {
                item = await new DatabaseMethod<DS_Trinh>(_context).GetOjectFromDBAsync(vm.ID);
                if (item == null) ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            }
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(item);
        }
    }
}
