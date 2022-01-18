using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult LevelInfo(TabViewModel model, bool isCurrent, int? trinhID, string detailedTitle)
        {
            if (model.ID == null)
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
            var columnsToSave = new List<string> { "Trinh", "DiemTru", "Diem_PB", "TL_VoDich", "TL_ChungKet", "TL_BanKet", "TL_TuKet", "TL_Bang" };
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
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var model = (from vdv in _context.DS_VDVs
                             where vdv.Ten_Tat.StartsWith(prefix)
                             select new
                             {
                                 label = vdv.Ten_Tat,
                                 val = vdv.Id
                             }).ToList();

            return Json(model);
        }
    }
}
