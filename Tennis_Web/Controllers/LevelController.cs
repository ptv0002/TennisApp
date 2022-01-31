using DataAccess;
using Library;
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
    public class LevelController : Controller
    {
        private readonly TennisContext _context;
        public LevelController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult LevelInfo(TabViewModel model, bool isCurrent, int  trinhID, string detailedTitle)
        {
            if (model.ID == 0)
            {
                // Assign default value for first time access
                model = new TabViewModel
                {
                    ActiveTab = Tab.Parameter,
                    IsCurrent = isCurrent,
                    ID = trinhID,
                    DetailedTitle = detailedTitle
                };
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateParameter(DS_Trinh item)
        {
            // Find and update Parameters from DS_Trinh
            item.TL_Bang = 100 - item.TL_VoDich - item.TL_ChungKet - item.TL_BanKet - item.TL_TuKet;
            var columnsToSave = new List<string> { "Trinh", "Diem_Tru", "Diem_PB", "TL_VoDich", "TL_ChungKet", "TL_BanKet", "TL_TuKet", "TL_Bang" };
            var result = new DatabaseMethod<DS_Trinh>(_context).SaveObjectToDB(item.Id, item, columnsToSave);
            _context.SaveChanges();
            var temp = _context.DS_Giais.Find(item.ID_Giai);
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.Parameter,
                IsCurrent = true,
                ID = item.Id,
                DetailedTitle = "Giải " + temp.Ten + " - Trình " + item.Trinh,
                Succeeded = result.Succeeded
            };

            // If save unsuccessfully, view error and display View with "item" 
            if (!result.Succeeded) vm.CurrentModel = item;
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction(nameof(LevelInfo), vm);
        }
        public IActionResult ChangePair(int id, int idTrinh)
        {
            var model = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == id).FirstOrDefault();
            if (model == null)
            {
                model = new DS_Cap { ID_Trinh = idTrinh };
            }
            ViewBag.DS_VDV = _context.DS_VDVs.Where(m => m.Tham_Gia == true).ToList();
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ChangePair(DS_Cap item)
        {
            var vdv1 = _context.DS_VDVs.Where(m => m.Ten_Tat == item.VDV1.Ten_Tat).FirstOrDefault();
            var vdv2 = _context.DS_VDVs.Where(m => m.Ten_Tat == item.VDV2.Ten_Tat).FirstOrDefault();
            DS_Cap obj;
            if (vdv1 != null && vdv1 != null)
            {
                obj = new()
                {
                    ID_Trinh = item.ID_Trinh,
                    ID_Vdv1 = vdv1.Id,
                    ID_Vdv2 = vdv2.Id,
                    Ma_Cap = item.Ma_Cap
                };
            }
            else
            {
                ViewBag.DS_VDV = _context.DS_VDVs.Where(m => m.Tham_Gia == true).ToList();
                ModelState.AddModelError(string.Empty, "Thông tin nhập không chính xác!");
                return PartialView(item);
            }
            var columnsToSave = new List<string> { "ID_Vdv1", "ID_Vdv2", "Ma_Cap", "ID_Trinh" };
            var result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(item.Id, obj, columnsToSave);
            _context.SaveChanges();
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == item.ID_Trinh).FirstOrDefault();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.Pair,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
            };
            return RedirectToAction(nameof(LevelInfo), vm);
        }
        public IActionResult DeletePair(string id)
        {
            var intId = Convert.ToInt32(id);
            var item = _context.DS_Caps.Find(intId);
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == item.ID_Trinh).FirstOrDefault();
            _context.Remove(item);
            _context.SaveChanges();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = Tab.Pair,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
            };
            return RedirectToAction(nameof(LevelInfo), vm);
        }
    }
}
