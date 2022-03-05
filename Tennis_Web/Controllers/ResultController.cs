using DataAccess;
using Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class ResultController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public ResultController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> UpdateTableAsync(RoundTabViewModel model)
        {
            var matches = model.DS_Tran;
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == matches.Find(m => m.ID_Trinh != 0).ID_Trinh);
            // Find and update result for Matches
            bool result = false;
            if (ModelState.IsValid)
            {
                var columnsToSave = new List<string> { "Kq_1", "Kq_2" };
                bool tableFilled = matches.Where(m => m.Ma_Vong == 8).All(m => (m.Kq_1 + m.Kq_2) > 0);
                bool tableChanges = !IsTheSame(matches.Where(m => m.Ma_Vong == 7).ToList(), temp.Id);
                bool playoffEmpty = matches.Any(m => m.Ma_Vong == 7 && (m.ID_Cap1 == null || m.ID_Cap2 == null));

                FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/MatchGenParam.json");
                var file = await JsonSerializer.DeserializeAsync<List<MatchGeneratorViewModel>>(fileStream);
                fileStream.Dispose();
                if (tableChanges)
                {
                    foreach (var match in matches.Where(m => m.Ma_Vong == 8).ToList())
                    {
                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, columnsToSave).Succeeded;
                        if (!result) break;
                    }
                    if (result) _context.SaveChanges();
                    // ========================= Rank pairs =========================

                    var tables = _context.DS_Bangs.Where(m => m.ID_Trinh == temp.Id);
                    foreach (var table in tables)
                    {
                        var rankedPairs = new TennisMethod(_context).Rank_Full(temp.Id, table.Ten);
                        foreach (var pair in rankedPairs)
                        {
                            result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xep_Hang", "Tran_Thang" }).Succeeded;
                            if (!result) break;
                        }
                    }
                    if (result) _context.SaveChanges();

                }
                //if (!playoffEmpty)
                //{
                //    foreach (var match in matches.Where(m => m.Ma_Vong == 7).ToList())
                //    {
                //        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, columnsToSave).Succeeded;
                //        if (!result) break;
                //    }
                //}
                //foreach (var level in file)
                //{
                //    if (level.PlayOff1 > 0)
                //    if (level.PlayOff2 > 0)
                //    {
                //        if (level.ChosenPerTable.se)
                //        }
                //}
                //bool uniqueRanking =
                //    // ========================= Get ranking and put in playoff =========================
                //    if (tableFilled && playoffEmpty)
                //{

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
