using AspNetCoreHero.ToastNotification.Abstractions;
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
        public readonly INotyfService _notyf;
        public ParameterViewComponent(TennisContext context, INotyfService notyf) 
        {
            _context = context;
            _notyf = notyf;
        }
        public async Task<IViewComponentResult> InvokeAsync(TournamentTabViewModel vm)
        {
            var item = (DS_Trinh)vm.CurrentModel;
            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công");
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi");

            if (vm.Succeeded == true || vm.Succeeded == null)
            {
                item = await new DatabaseMethod<DS_Trinh>(_context).GetOjectFromDBAsync(vm.ID);
                if (item == null) ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            }
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(item);
        }
    }
}
