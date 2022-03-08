﻿using DataAccess;
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
                //var uniqueRanking = _context.DS_Caps.Where(m => m.ID_Trinh == model.ID_Trinh).GroupBy(m => m.Xep_Hang).All(m => m.Count() == 1);
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
                        int count = -1;
                        // ================== Add players to Playoff2 rounds ==================
                        for (int i = 0; i < P2match.Count; i++)
                        {
                            P2match[i].ID_Cap1 = considerList[++count].Id;
                            P2match[i].ID_Cap2 = considerList[++count].Id;
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

                // Arrange 1st special round
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
                    var result1 = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                    var result2 = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Boc_Tham" }).Succeeded;
                    result = result1 && result2;
                    if (!result) break;
                }
                if (result) _context.SaveChanges();
                // Playoff 2 has Pair ids then update, else ignore
                if (matches.Where(m => m.Ma_Vong == 7).All(m => m.ID_Cap1 != null && m.ID_Cap2 != null))
                {
                    foreach (var match in matches.Where(m => m.Ma_Vong == 7).ToList())
                    {
                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                        if (!result) break;
                    }
                }

            }
            else result = false;
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
        public List<DS_Cap> RankPlayoff (Dictionary<char,int> straight2Special, List<char> chosenPlayoff, List<char> chosen2, int idTrinh, int PlayoffNum)
        {
            var considerList = new List<DS_Cap>();
            foreach (var table in chosenPlayoff)
            {
                // Any pair has repeated ranking then returns null
                if (_context.DS_Caps.Where(m => m.ID_Trinh == idTrinh && m.DS_Bang.Ten == table).GroupBy(m => m.Xep_Hang).Any(m => m.Count() > 1)) return null;
                var pair = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => m.ID_Trinh == idTrinh).FirstOrDefault(m => m.DS_Bang.Ten == table && m.Xep_Hang == (straight2Special[table] + 1));
                considerList.Add(pair);
            }
            considerList = considerList.OrderByDescending(m => m.Tran_Thang).ThenByDescending(m => m.Hieu_so).ToList();
            var returnList = new List<DS_Cap>();
            for (int i = 0; i < PlayoffNum; i++)
            {
                returnList.Add(considerList[i]);
                if (chosen2 != null) chosen2.Remove(considerList[i].DS_Bang.Ten);
            }
            return returnList;
        }
        public IActionResult UpdateSpecial()
        {
            return View();
        }
        //public bool IsTheSame(List<DS_Tran> source, int idTrinh)
        //{
        //    var oldMatches = _context.DS_Trans.Where(m => m.ID_Trinh == idTrinh).ToList();
        //    foreach (var newMatch in source)
        //    {
        //        var oldMatch = oldMatches.Find(m => m.Id == newMatch.Id);
        //        if (!(newMatch.Kq_1 == oldMatch.Kq_1 && newMatch.Kq_2 == oldMatch.Kq_2)) return false;
        //    }
        //    return true;
        //}
    }
}
