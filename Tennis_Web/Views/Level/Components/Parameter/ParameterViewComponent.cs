using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Level.Components.Parameter
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
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            DS_Trinh item = new();
            if ((string)TempData["ParameterInfo"] != null) item = JsonSerializer.Deserialize<DS_Trinh>((string)TempData["ParameterInfo"]);
            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công");
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");

            if (vm.Succeeded != false)
            {
                item = _context.DS_Trinhs.Find(vm.ID);
                if (item == null) ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            }
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(item);
        }
    }
}
