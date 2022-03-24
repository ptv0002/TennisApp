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
        /// <summary>
        /// 1. Xếp hạng các cặp trong các bảng
        /// 2. Có đầy đủ kết quả vòng bảng  --> Cập nhật vòng Playoff
        /// 3. Các cặp được chọn cập nhật vào vòng trong
        /// 4. Có đầy đủ kết quả vòng bảng + Playoff --> Cập nhật lại vòng đặc biệt và khóa vòng bảng (Chỉ cho Admin cập nhật lại)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> PerformRankingAsync(RoundTabViewModel model)
        {
            bool result = UpdateResult_Table(model);
            // If update successfully, proceeds
            if (result)
            {
                FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/MatchGenParam.json");
                var matchParam = (await JsonSerializer.DeserializeAsync<List<MatchGeneratorViewModel>>(fileStream)).Find(m => m.ID_Trinh == model.ID_Trinh);
                fileStream.Dispose();

                // 1. ========================= Rank pairs =========================

                var tables = _context.DS_Bangs.Where(m => m.ID_Trinh == model.ID_Trinh);
                foreach (var table in tables)
                {
                    var rankedPairs = new MatchCalculation(_context).Rank_Full(model.ID_Trinh, table.Ten);
                    foreach (var pair in rankedPairs)
                    {
                        result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xep_Hang", "Tran_Thang", "Hieu_so" }).Succeeded;
                        if (!result) break;
                    }
                }
                if (result) _context.SaveChanges();

                // 2. ========================= Get ranking and put in playoff =========================
                var playoff1 = new List<DS_Cap>();
                result = ToPlayoff(playoff1, matchParam, model);

                // ================== Add players to First Special ==================
                result = result && await ToSpecialAsync(playoff1, matchParam, model);
                // Save all changes to DB
                if (result) _context.SaveChanges();
            }
            return TabVMGenerator(model.ID_Trinh, result, Tab.Table, "");
        }
    public IActionResult UpdateTable(RoundTabViewModel model)
        {
            bool result = UpdateResult_Table(model);
            return TabVMGenerator(model.ID_Trinh, result, Tab.Table,"");
        }
        public IActionResult UpdateSpecial(RoundTabViewModel model)
        {
            // Find and update result for Matches
            bool result = false;
            var matches = model.DS_Tran.Where(m => m.Ma_Vong <= 6).ToList();
            var scoreList = new List<DS_Diem>();
            if (ModelState.IsValid)
            {
                for (int i = matches.Max(m => m.Ma_Vong); i > 1; i--)
                {
                    var rankedPairs = new MatchCalculation(_context).Special_Update(matches, i);
                }
                foreach (var match in matches)
                {
                    if (match.Kq_1 + match.Kq_2 > 0 && match.ID_Cap1 != null && match.ID_Cap2 != null)
                    {
                        // Calculate points for Round 1 to 3, if any
                        if (4 <= match.Ma_Vong && match.Ma_Vong <= 6) scoreList.AddRange(new ScoreCalculation(_context).Head2Head_Point(match));
                        else scoreList.AddRange(new ScoreCalculation(_context).Special_Point(match));
                    }
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2", "ID_Cap1", "ID_Cap2"}).Succeeded;
                    if (!result) break;
                }
                if (result)
                {
                    // Add or update score for special rounds
                    UpdateScore(scoreList);
                }
            }
            else result = false;
            // Save all changes to DB
            if (result) _context.SaveChanges();
            return TabVMGenerator(model.ID_Trinh, result, Tab.Special, "");
        }
        public IActionResult ResetSpecial(int id)
        {
            bool result = ResetResult_Special(id);
            return TabVMGenerator(id, result, Tab.Table, "");
        }
        public IActionResult UpdatePlayoff(RoundTabViewModel model)
        {
            // Find and update result for Matches
            bool result = false;
            string msg = "";
            var matches = model.DS_Tran.Where(m => m.Ma_Vong == 7);
            var scoreList = new List<DS_Diem>();
            if (ModelState.IsValid)
            {
                // Save results to DB and calculate ranking
                foreach (var match in matches)
                {
                    scoreList.AddRange(new ScoreCalculation(_context).Head2Head_Point(match));
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                    if (!result) break;
                }
                if (result)
                {
                    // Add or update score for playoff round
                    UpdateScore(scoreList);
                }
            }
            else result = false;
            if (result && matches.All(m => m.Kq_1 + m.Kq_2 > 0) && (User.IsInRole("Manager") || User.IsInRole("Admin")))
            {
                var roundNum = _context.DS_Trans.Where(m => m.Ma_Vong <= 6 && m.ID_Trinh == model.ID_Trinh).Max(m => m.Ma_Vong);
                // Get first special round
                var special1stRound = _context.DS_Trans.Where(m => m.Ma_Vong == roundNum && m.ID_Trinh == model.ID_Trinh && m.ID_Cap2 == null)
                    .OrderBy(m => m.Ma_Tran).ToList();


                // If there's any special 1st round with empty slot, proceeds
                if (special1stRound.Any())
                {
                    var winId = matches.Select(m => m.Kq_1 > m.Kq_2 ? m.ID_Cap1 : m.ID_Cap2).ToList();
                    var winPairs = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => winId.Contains(m.Id));
                    foreach (var match in special1stRound)
                    {
                        var code = match.Chon_Cap_2;
                        DS_Cap pair = new();
                        int j = 1;
                        while (j < match.Chon_Cap_2.Length)
                        {
                            pair = winPairs.FirstOrDefault(m => m.DS_Bang.Ten == code[j] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                            // Founf pair with required Table_Ranking combo
                            if (pair != null)
                            {
                                match.ID_Cap2 = pair.Id;
                                break;
                            }
                            // No pair found with required Table_Ranking combo
                            // Increment to find next
                            j++;
                        }
                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "ID_Cap2" }).Succeeded;
                        if (!result) break;
                    }

                }
                else
                {
                    msg = "Không thể thêm cặp playoff vào vòng đặc biệt. Xóa kết quả vòng đặc biệt rồi thử lại";
                    result = false;
                }
            }
            if (result) _context.SaveChanges();
            return TabVMGenerator(model.ID_Trinh, result, Tab.Table, msg);
        }
        async Task<bool> ToSpecialAsync(List<DS_Cap> playoff1, MatchGeneratorViewModel matchParam, RoundTabViewModel model)
        {
            bool result = false;
            var totalPair = matchParam.ChosenPerTable.Sum(m => m.Chosen) + matchParam.PlayOff1 + matchParam.PlayOff2;
            var straight2Special = matchParam.ChosenPerTable.ToDictionary(x => x.Table, y => y.Chosen);
            FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/Special1stRound.json");
            var placement = (await JsonSerializer.DeserializeAsync<List<Special1stRound>>(fileStream)).Find(m => m.PairNum == totalPair && m.TableNum == straight2Special.Count);
            fileStream.Dispose();
            // Reset all special rounds' value before proceeding
            result = ResetResult_Special(model.ID_Trinh);

            if (placement != null && result)
            {
                var allPairs = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => m.ID_Trinh == model.ID_Trinh).ToList();
                var ds_cap = new List<DS_Cap>();

                // Generate straight to special list 
                foreach (var table in straight2Special.Keys)
                {
                    for (int i = 1; i <= straight2Special[table]; i++)
                    {
                        ds_cap.AddRange(allPairs.Where(m => m.DS_Bang.Ten == table && m.Xep_Hang == i));
                    }
                }
                // Add playoff 1 players if any
                if (playoff1.Any()) ds_cap.AddRange(playoff1);
                var roundNum = _context.DS_Trans.Where(m => m.Ma_Vong <= 6 && m.ID_Trinh == model.ID_Trinh).Max(m => m.Ma_Vong);
                // Get first special round
                var special1stRound = _context.DS_Trans.Where(m => m.Ma_Vong == roundNum && m.ID_Trinh == model.ID_Trinh).OrderBy(m => m.Ma_Tran).ToList();
                for (int i = 0; i < special1stRound.Count; i++)
                {
                    // Assign pair to Pair 1 for the match
                    var code = placement.P1_P2_Pair.ElementAt(i).Key;
                    special1stRound[i].Chon_Cap_1 = code;
                    var pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                    // Ranking is unique => Assign pair to match
                    // Ignore otherwise
                    if (pair.Count() == 1) special1stRound[i].ID_Cap1 = pair.First().Id;

                    // Assign pair to Pair 2 for the match
                    code = placement.P1_P2_Pair.ElementAt(i).Value;
                    special1stRound[i].Chon_Cap_2 = code;

                    // If code.length > 2 => pair pulls from playoff (null in this case)
                    if (code.Length > 2)
                    {
                        int j = 1;
                        while (j < code.Length)
                        {
                            pair = ds_cap.Where(m => m.DS_Bang.Ten == code[j] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                            // Founf pair with required Table_Ranking combo
                            if (pair.Any())
                            {
                                // Ranking is unique => Assign pair to match
                                if (pair.Count() == 1) special1stRound[i].ID_Cap2 = pair.First().Id;
                                // Else ranking is not unique, but it's possible to assign pair from code[j] table after more Ranking
                                // => Ignore
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
                        pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                        // Ranking is unique => Assign pair to match
                        // Ignore otherwise
                        if (pair.Count() == 1) special1stRound[i].ID_Cap2 = pair.First().Id;
                    }
                    var columns2Save = new List<string> { "ID_Cap1", "ID_Cap2", "Chon_Cap_1", "Chon_Cap_2" };
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(special1stRound[i].Id, special1stRound[i], columns2Save).Succeeded;
                    if (!result) break;
                }
            }
            return result;
        }
        bool ToPlayoff(List<DS_Cap> playoff1, MatchGeneratorViewModel matchParam, RoundTabViewModel model)
        {
            bool result = false;
            var straight2Special = matchParam.ChosenPerTable.ToDictionary(x => x.Table, y => y.Chosen);
            // Check if tables have unique ranking
            var considerList = new List<DS_Cap>();
            var calculatePlayoff = matchParam.PlayOff1 + matchParam.PlayOff2 > 0;
            var tables = matchParam.ChosenPerTable.Where(m => m.Playoff).Select(m => m.Table);
            foreach (var table in tables)
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
                considerList = considerList.OrderByDescending(m => m.Tran_Thang).ThenByDescending(m => m.Hieu_so).ThenBy(m => m.Boc_Tham).ToList();

                // If there's playoff 1 rounds then proceeds
                if (matchParam.PlayOff1 > 0)
                {
                    var returnList = new List<DS_Cap>();
                    for (int i = 0; i < matchParam.PlayOff1; i++)
                    {
                        playoff1.Add(considerList[i]);
                    }
                    considerList.RemoveRange(0, matchParam.PlayOff1);
                    result = true;
                }
                // If there's playoff 2 rounds then proceeds
                if (matchParam.PlayOff2 > 0)
                {
                    var P2match = model.DS_Tran.Where(m => m.Ma_Vong == 7).OrderBy(m => m.Ma_Tran).ToList();
                    considerList = considerList.OrderBy(m => m.DS_Bang.Ten).ToList();
                    int count = 0;
                    // ================== Add players to Playoff2 rounds ==================
                    for (int i = 0; i < P2match.Count; i++)
                    {
                        P2match[i].ID_Cap1 = considerList[count++].Id;
                        P2match[i].ID_Cap2 = considerList[count++].Id;
                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(P2match[i].Id, P2match[i], new List<string> { "ID_Cap1", "ID_Cap2" }).Succeeded;
                        if (!result) break;
                    }
                }
            }
            return result;
        }
        
        bool UpdateResult_Table(RoundTabViewModel model)
        {
            // Find and update result for Matches
            bool result = false;
            if (ModelState.IsValid)
            {
                // Save results to DB and calculate ranking
                foreach (var match in model.DS_Tran.Where(m => m.Ma_Vong == 8))
                {
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                    if (!result) break;
                }
                // Nếu đã cập nhật hết dữ liệu, tự động tính xếp hạng luông
                var lketqua = _context.DS_Trans.Where(m => m.Ma_Vong == 8 && m.ID_Trinh == model.ID_Trinh && (m.Kq_1 + m.Kq_2) == 0);
                if (lketqua.Any())
                {// Chưa cập nhật đầy đủ kết quả
                    foreach (var pair in model.DS_Cap)
                    {
                        result = result && new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Tran_Thang", "Hieu_so", "Boc_Tham" }).Succeeded;
                        if (!result) break;
                    }
                }
                else { 
                    //PerformRankingAsync(model)
                    ; }
            }
            else result = false;
            if (result) _context.SaveChanges();

            //var scoreList = new List<DS_Diem>();
            //foreach (var table in _context.DS_Bangs.Where(m => m.ID_Trinh == model.ID_Trinh))
   
            
            //{
            //    scoreList.AddRange(new ScoreCalculation(_context).TableAndPositive_Point(model.ID_Trinh, table.Ten));
            //}
            //// Add or update score for Table rounds
            //UpdateScore(scoreList);
            //_context.SaveChanges();

            return result;
        }
        bool ResetResult_Special(int idTrinh)
        {
            bool result = false;
            // Get first special round
            var special1stRound = _context.DS_Trans.Where(m => m.Ma_Vong <= 6 && m.ID_Trinh == idTrinh).ToList();
            // Delete all scores if results are reset
//            var scores = _context.DS_Diems.AsEnumerable().Where(m => m.ID_Vong <= 6 && special1stRound.SelectMany(s => new[] { s.ID_Cap1, s.ID_Cap2 }).Contains(m.ID_Cap));
//            _context.RemoveRange(scores);
            foreach (var match in special1stRound)
            {
                match.ID_Cap1 = match.ID_Cap2 = null;
                result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "ID_Cap1", "ID_Cap2" }).Succeeded;
                if (!result) break;
            }
            if (result) _context.SaveChanges();
            return result;
        }
        void UpdateScore(List<DS_Diem> scoreList)
        {
            foreach (var score in scoreList)
            {
                if (_context.DS_Diems.Any(m => m.ID_Cap == score.ID_Cap && m.ID_Vong == score.ID_Vong))
                {
                    var temp = _context.DS_Diems.FirstOrDefault(m => m.ID_Cap == score.ID_Cap && m.ID_Vong == score.ID_Vong);
                    temp.Diem = score.Diem;
                    _context.Update(temp);
                }
                // If not, add new instance
                else _context.Add(score);
            }
        }
        IActionResult TabVMGenerator(int idTrinh, bool result, Tab tabName, string msg)
        {
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).FirstOrDefault(m => m.Id == idTrinh);
            var vm = new TabViewModel
            {
                ActiveTab = tabName,
                IsCurrent = true,
                ID = temp.Id,
                DetailedTitle = "Giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh,
                Succeeded = result,
                ErrorMsg = msg
            };
            // If save successfully, view error and display View with model from DB 
            return RedirectToAction("MatchInfo", "Match", vm);
        }
    }
}
