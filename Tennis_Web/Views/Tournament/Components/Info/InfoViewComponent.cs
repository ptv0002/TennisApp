﻿using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Tournament.Components.Info
{
    public class InfoViewComponent : ViewComponent
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public InfoViewComponent(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IViewComponentResult Invoke(TabViewModel vm)
        {
            DS_Giai item = new();
            //if ((string)TempData["TournamentInfo"] != null) item = JsonSerializer.Deserialize<DS_Giai>((string)TempData["TournamentInfo"]);
            //if (vm.CurrentModel != null) item = JsonSerializer.Deserialize<DS_Giai>(vm.CurrentModel);
            if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công!");
            else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");

            if (vm.Succeeded != false)
            {
                item = _context.DS_Giais.Find(vm.ID);
                if (vm.ID==0) ModelState.AddModelError(string.Empty, "Cần lưu thay đổi trước khi thêm trình!");
            }
           
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(item);
        }
    }
}
