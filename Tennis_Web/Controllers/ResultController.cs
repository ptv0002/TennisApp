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
    public class ResultController : Controller
    {
        private readonly TennisContext _context;
        public ResultController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UpdateTable(TableTabViewModel model)
        {
            var matches = model.DS_Tran;
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == matches.Find(m => m.ID_Trinh != 0).ID_Trinh);
            // Find and update result for Matches
            bool result = false;
            var columnsToSave = new List<string> { "Kq_1", "Kq_2" };
            if (ModelState.IsValid)
            {
                //bool tableFilled = matches.Where(m => m.DS_Vong.Ma_Vong == 7).All(m => (m.Kq_1 + m.Kq_2) > 0);
                //bool tableChanges = !IsTheSame(matches.Where(m => m.DS_Vong.Ma_Vong == 7).ToList(), temp.Id);
                //if (tableChanges)
                //{
                //    update:
                //    foreach (var match in matches)
                //    {
                //        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, columnsToSave).Succeeded;
                //        if (!result) break;
                //    }
                //    if (result) _context.SaveChanges();
                //    if (tableFilled)
                //    {
                //        // ========================= Rank pairs here =========================

                //    }
                //}
                //else
                //{
                //    matches = matches.Where(m => m.DS_Vong.Ma_Vong == 8).ToList();
                //    foreach (var match in matches)
                //    {
                //        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, columnsToSave).Succeeded;
                //        if (!result) break;
                //    }
                //}
                // ================== Add players to Special rounds ==================
            }
            else result = false;
            

            var vm = new TabViewModel
            {
                ActiveTab = Tab.Table,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
                Succeeded = result
            };
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction("MatchInfo", "Match", vm);
        }
        public IActionResult UpdateSpecial()
        {
            return View();
        }
        public bool IsTheSame(List<DS_Tran> source, int idTrinh)
        {
            var oldMatches = _context.DS_Trans.Where(m => m.ID_Trinh == idTrinh).ToList();
            foreach (var newMatch in source)
            {
                var oldMatch = oldMatches.Find(m => m.Id == newMatch.Id);
                if (!(newMatch.Kq_1 == oldMatch.Kq_1 && newMatch.Kq_2 == oldMatch.Kq_2)) return false;
            }
            return true;
        }
    }
}
