using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Views.Tournament.Components.Player
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
            List<DS_VDV> players = new();
            //if ((string)TempData["PlayerList"] != null) players = JsonSerializer.Deserialize<List<DS_VDV>>((string)TempData["PlayerList"]);
            //if (vm.CurrentModel != null) players = JsonSerializer.Deserialize<List<DS_VDV>>(vm.CurrentModel);
            if (vm.IsCurrent == true)
            {
                if (vm.Succeeded == true) _notyf.Success("Lưu thay đổi thành công!");
                else if (vm.Succeeded == false) _notyf.Error("Có lỗi xảy ra khi đang lưu thay đổi!");

                if (vm.Succeeded != false) players = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
                ViewBag.ID_Giai = vm.ID;
            }
            else
            {
                // Display for previous tournament
                // Get all levels from given tournament
                var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == vm.ID).Select(m => m.Id);
                // Get all pairs with Level Id from the level id list
                var vdv_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2 });
                // Get all players with from Player Id found in Player1 and Player2 lists
                players = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id)).OrderByDescending(m => m.Diem).ThenByDescending(m => m.Diem_Cu).ToList();

            }
            ViewBag.IsCurrent = vm.IsCurrent;
            return View(players);
        }
    }
}
