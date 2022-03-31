using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public IActionResult LevelInfo(TabViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateParameter(DS_Trinh item)
        {
            // Find and update Parameters from DS_Trinh
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
            TempData["ParameterInfo"] = JsonSerializer.Serialize(item);
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction(nameof(LevelInfo), vm);
        }
        public List<DS_VDV> PopulateAutoComplete(int idTrinh)
        {
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == _context.DS_Trinhs.Find(idTrinh).ID_Giai).Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            //var vdv_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2 });
            // Get all players with from Player Id found in Player1 and Player2 lists
            //var players = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id));

            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));

            return _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).ToList();
        }
        public IActionResult UpdatePair(int id, int idTrinh)
        {
            var model = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == id).FirstOrDefault();
            if (model == null)
            {
                model = new DS_Cap { ID_Trinh = idTrinh };
            }

            ViewBag.DS_VDV = PopulateAutoComplete(idTrinh);
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult UpdatePair(DS_Cap item)
        {
            var vdv1 = _context.DS_VDVs.Where(m => m.Ten_Tat == item.VDV1.Ten_Tat).FirstOrDefault();
            var vdv2 = _context.DS_VDVs.Where(m => m.Ten_Tat == item.VDV2.Ten_Tat).FirstOrDefault();
            DS_Cap obj;
            if (vdv1 != null && vdv2 != null)
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
                ViewBag.DS_VDV = PopulateAutoComplete(item.ID_Trinh);
                ModelState.AddModelError(string.Empty, "Tên tắt VĐV không chính xác!");
                return PartialView(item);
            }
            var columnsToSave = new List<string> { "ID_Vdv1", "ID_Vdv2", "Ma_Cap", "ID_Trinh" };
            var result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(item.Id, obj, columnsToSave);
            if (result.Succeeded) _context.SaveChanges();
            else
            {
                ViewBag.DS_VDV = PopulateAutoComplete(item.ID_Trinh);
                ModelState.AddModelError(string.Empty, result.Message);
                return PartialView(item);
            }
            new ScoreCalculation(_context).Point_Deposit(item.ID_Trinh);
            return TabVMGenerator(Tab.Pair, item.ID_Trinh);
        }
        public IActionResult DeletePair(string id)
        {
            var pair = _context.DS_Caps.Find(Convert.ToInt32(id));
            var matches = _context.DS_Trans.Where(m => m.ID_Cap1 == pair.Id || m.ID_Cap2 == pair.Id);
            _context.RemoveRange(matches);
            _context.Remove(pair);
            _context.SaveChanges();
            new ScoreCalculation(_context).Point_Deposit(pair.ID_Trinh);
            return TabVMGenerator(Tab.Pair, pair.ID_Trinh);
        }
        public IActionResult TabVMGenerator (Tab tabName, int idTrinh)
        {
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == idTrinh).FirstOrDefault();
            // Assign value for view model
            var vm = new TabViewModel
            {
                ActiveTab = tabName,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
            };
            return RedirectToAction(nameof(LevelInfo), vm);
        }
    }
}
