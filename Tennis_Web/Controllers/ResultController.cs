using DataAccess;
using Library;
using Library.FileInitializer;
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
        public async Task<IActionResult> PerformRankingAsync (RoundTabViewModel model)
        {
            bool result = UpdateResults(model);
            // If update successfully, proceeds
            if (result)
            {
                var matches = model.DS_Tran;
                FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/MatchGenParam.json");
                var matchParam = (await JsonSerializer.DeserializeAsync<List<MatchGeneratorViewModel>>(fileStream)).Find(m => m.ID_Trinh == model.ID_Trinh);
                fileStream.Dispose();

                // ========================= Rank pairs =========================

                var tables = _context.DS_Bangs.Where(m => m.ID_Trinh == model.ID_Trinh);
                foreach (var table in tables)
                {
                    var rankedPairs = new TennisMethod(_context).Rank_Full(model.ID_Trinh, table.Ten);
                    foreach (var pair in rankedPairs)
                    {
                        result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xep_Hang", "Tran_Thang", "Hieu_so" }).Succeeded;
                        if (!result) break;
                    }
                }
                if (result) _context.SaveChanges();

                // ========================= Get ranking and put in playoff =========================
                var playoff1 = new List<DS_Cap>();
                var straight2Special = matchParam.ChosenPerTable.ToDictionary(x => x.Table, y => y.Chosen);
                var chosenPlayoff = matchParam.ChosenPerTable.Where(m => m.Playoff).Select(m => m.Table).ToList();

                // Check if tables have unique ranking
                var considerList = new List<DS_Cap>();
                var calculatePlayoff = true;
                foreach (var table in chosenPlayoff)
                {
                    // Any pair has repeated ranking then returns null
                    if (_context.DS_Caps.Where(m => m.ID_Trinh == model.ID_Trinh && m.DS_Bang.Ten == table).GroupBy(m => m.Xep_Hang).Any(m => m.Count() > 1))
                    {
                        calculatePlayoff = false;
                        break; 
                    }
                    var pair = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => m.ID_Trinh == model.ID_Trinh).FirstOrDefault(m => m.DS_Bang.Ten == table && m.Xep_Hang == (straight2Special[table] + 1));
                    considerList.Add(pair);
                }
                if (calculatePlayoff)
                {
                    considerList = considerList.OrderByDescending(m => m.Tran_Thang).ThenByDescending(m => m.Hieu_so).ToList();

                    // If there's playoff 1 rounds then proceeds
                    if (matchParam.PlayOff1 > 0)
                    {
                        var returnList = new List<DS_Cap>();
                        for (int i = 0; i < matchParam.PlayOff1; i++)
                        {
                            playoff1.Add(considerList[i]);
                            considerList.Remove(considerList[i]);
                        }
                    }
                    // If there's playoff 2 rounds then proceeds
                    if (matchParam.PlayOff2 > 0)
                    {
                        var P2match = matches.Where(m => m.Ma_Vong == 7).OrderBy(m => m.Ma_Tran).ToList();
                        considerList = considerList.OrderBy(m => m.DS_Bang.Ten).ToList();
                        int count = 0;
                        // ================== Add players to Playoff2 rounds ==================
                        for (int i = 0; i < P2match.Count; i++)
                        {
                            P2match[i].ID_Cap1 = considerList[count++].Id;
                            P2match[i].ID_Cap2 = considerList[count++].Id;
                            P2match[i].Kq_1 = P2match[i].Kq_2 = 0;
                            result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(P2match[i].Id, P2match[i], new List<string> { "ID_Cap1", "ID_Cap2" }).Succeeded;
                            if (!result) break;
                        }
                    }
                }
                // ================== Add players to First Special ==================
                var totalPair = matchParam.ChosenPerTable.Sum(m => m.Chosen) + matchParam.PlayOff1 + matchParam.PlayOff2;
                fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/Special1stRound.json");
                var placement = (await JsonSerializer.DeserializeAsync<List<Special1stRound>>(fileStream)).Find(m => m.PairNum == totalPair && m.TableNum == straight2Special.Count());
                fileStream.Dispose();

                if (placement != null)
                {
                    var ds_cap = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => m.ID_Trinh == model.ID_Trinh).ToList();
                    var roundNum = _context.DS_Trans.Where(m => m.Ma_Vong <= 6).Max(m => m.Ma_Vong);
                    // Get first special round
                    var special1stRound = _context.DS_Trans.Where(m => m.Ma_Vong == roundNum).OrderBy(m => m.Ma_Tran).ToList();
                    for (int i = 0; i < special1stRound.Count; i++)
                    {
                        // Assign pair to Pair 1 for the match
                        var code = placement.P1_P2_Pair.ElementAt(i).Key;
                        special1stRound[i].Chon_Cap_1 = code;
                        var pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == code[0]);
                        // Ranking is unique => Assign pair to match
                        // Ignore otherwise
                        if (pair.Count() == 1) special1stRound[i].ID_Cap1 = pair.First().Id;

                        // Assign pair to Pair 2 for the match
                        code = placement.P1_P2_Pair.ElementAt(i).Value;
                        special1stRound[i].Chon_Cap_2 = code;

                        // If code.length > 2 => pair pulls from playoff
                        if (code.Length > 2)
                        {
                            int j = 1;
                            while (j < code.Length)
                            {
                                pair = ds_cap.Where(m => m.DS_Bang.Ten == code[j] && m.Xep_Hang == code[0]);
                                // Founf pair with required Table_Ranking combo
                                if (pair.Count() >= 1)
                                {
                                    // Ranking is unique => Assign pair to match
                                    if (pair.Count() == 1) special1stRound[i].ID_Cap2 = pair.First().Id;
                                    // Else ranking is not unique, but it's possible to assign pair from code[j] table after more Ranking
                                    // => Ignore

                                    chosenPlayoff.Remove(code[j]);
                                    break;
                                }
                                // No pair found with required Table_Ranking combo
                                // Increment to find next
                                j++;
                            }
                        }
                        // Else => unique pair from template
                        else
                        {
                            pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == code[0]);
                            // Ranking is unique => Assign pair to match
                            // Ignore otherwise
                            if (pair.Count() == 1) special1stRound[i].ID_Cap2 = pair.First().Id;
                        }
                        var columns2Save = new List<string> { "ID_Cap1", "ID_Cap2", "Chon_Cap_1", "Chon_Cap_2" };
                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(special1stRound[i].Id, special1stRound[i], columns2Save).Succeeded;
                        if (!result) break;
                    }
                }
                // Save all changes to DB
                if (result) _context.SaveChanges();
            }
            return TabVMGenerator(model.ID_Trinh, result);
        }
        public IActionResult UpdateTable(RoundTabViewModel model)
        {
            bool result = UpdateResults(model);
            return TabVMGenerator(model.ID_Trinh, result);
        }
        public bool UpdateResults(RoundTabViewModel model)
        {
            var matches = model.DS_Tran;
            // Find and update result for Matches
            bool result = false;
            if (ModelState.IsValid)
            {
                // Save results to DB and calculate ranking
                foreach (var match in matches.Where(m => m.Ma_Vong == 8).ToList())
                {
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                    if (!result) break;
                }
                foreach (var pair in model.DS_Cap)
                {
                    result = result && new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Boc_Tham" }).Succeeded;
                    if (!result) break;
                }
                // Playoff 2 has Pair ids then update, else ignore
                if (matches.Where(m => m.Ma_Vong == 7).All(m => m.ID_Cap1 != null && m.ID_Cap2 != null))
                {
                    foreach (var match in matches.Where(m => m.Ma_Vong == 7).ToList())
                    {
                        result = result && new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                        if (!result) break;
                    }
                }
            }
            else result = false;
            if (result) _context.SaveChanges();
            return result;
        }
        public IActionResult TabVMGenerator(int idTrinh, bool result)
        {
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == idTrinh);
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
    }
}
