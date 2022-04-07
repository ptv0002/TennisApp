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
        public IActionResult Table_UpdateTable(RoundTabViewModel model)
        {
            bool result = Table_UpdateResult(model);
            return TabVMGenerator(model.ID_Trinh, result, Tab.Table, "");
        }
        /// <summary>
        /// Cập nhật kết quả thi đấu các bảng
        /// - Cập nhật kết quả vào DS_Tran - Danh sách trận đấu
        /// - Cập nhật điểm, hiệu số vào DS_Cap - Danh sách cặp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool Table_UpdateResult(RoundTabViewModel model)
        {
            // Find and update result for Matches
            bool result = false;
            if (ModelState.IsValid)
            {
                // Save results to DB 
                foreach (var match in model.DS_Tran.Where(m => m.Ma_Vong == 8))  // DS_Tran
                {
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2" }).Succeeded;
                    if (!result) break;
                }
                foreach (var pair in model.DS_Cap)      // DS_Cap
                {
                    result = result && new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Tran_Thang", "Hieu_so", "Boc_Tham" }).Succeeded;
                    if (!result) break;
                }
            }
            else result = false;
            if (result) _context.SaveChanges();
            return result;
        }
        /// <summary>
        /// 1. Xếp hạng các cặp trong các bảng
        /// 2. Đã có kết quả các trận đấu vòng bảng --> Tính điểm vòng bảng
        /// 3. Có đầy đủ kết quả vòng bảng  --> Cập nhật vòng Playoff
        /// 4. Các cặp được chọn cập nhật vào vòng trong
        /// 5. Có đầy đủ kết quả vòng bảng + Playoff --> Cập nhật lại vòng đặc biệt và khóa vòng bảng (Chỉ cho Admin cập nhật lại)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> Table_PerformRankingAsync(RoundTabViewModel model)
        {
            bool result = true;
            //bool result = Table_UpdateResult(model);
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
                // 2. ========================= Get Score if finnish Round =========================
                var lketqua = _context.DS_Trans.Where(m => m.Ma_Vong == 8 && m.ID_Trinh == model.ID_Trinh && (m.Kq_1 + m.Kq_2) == 0);
                if (! lketqua.Any()) { Table_Scoring(model);}
                // 3. ========================= Get ranking and put in playoff =========================
                var playoff1 = Table_ToPlayoff1(matchParam, model);
                result = Table_ToPlayoff2(matchParam, model);
                // ================== Add players to First Special ==================
                result = result && await Table_ToSpecialAsync(playoff1, matchParam, model);
                // Save all changes to DB
                if (result) _context.SaveChanges();
            }
            return TabVMGenerator(model.ID_Trinh, result, Tab.Table, "");
        }
        /// <summary>
        /// Tính điểm toàn bộ bảng trong trình
        /// Gồm 2 phần : 
        /// - Tính điểm Thưởng/Phạt
        /// - Tính điểm hệ số dương
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void Table_Scoring(RoundTabViewModel model)
        {
            var scoreList = new List<DS_Diem>();
            var tables = _context.DS_Bangs.Where(m => m.ID_Trinh == model.ID_Trinh);
            foreach (var table in tables)
            {
                scoreList.AddRange(new ScoreCalculation(_context).TableAndPositive_Point(model.ID_Trinh, table.Ten));
            }
            // Add or update score for Table rounds
            UpdateScore(scoreList);
        }
        List<DS_Cap> Table_ToPlayoff1(MatchGeneratorViewModel matchParam, RoundTabViewModel model)
        {
            if (matchParam.PlayOff1 <= 0) { return null; }
            List<DS_Cap> l_DSCap = new();
            List<DS_Cap> considerList = new();
            //var tables = matchParam.ChosenPerTable.Where(m => m.Playoff).Select(m => m.Table);
            var tables = matchParam.ChosenPerTable.Where(m => m.Playoff);
            foreach (var table in tables)
            {
                var pair = _context.DS_Caps.Include(m => m.DS_Bang)
                    .Where(m => m.ID_Trinh == model.ID_Trinh && !m.Phe_Duyet && !m.Xac_Nhan)
                    .FirstOrDefault(m => m.DS_Bang.Ten == table.Table && m.Xep_Hang == (table.Chosen + 1));
                considerList.Add(pair);
            }
            considerList = considerList.OrderByDescending(m => m.Tran_Thang).ThenByDescending(m => m.Hieu_so).ThenBy(m => m.Boc_Tham).ToList();
            // If there's playoff 1 rounds then proceeds
            for (int i = 0; i < matchParam.PlayOff1; i++)
            {
                l_DSCap.Add(considerList[i]);
            }
            return l_DSCap;
        }
        bool Table_ToPlayoff2(MatchGeneratorViewModel matchParam, RoundTabViewModel model)
        {
            if (matchParam.PlayOff2 <= 0) { return true; }
            bool result = true;
            var straight2Special = matchParam.ChosenPerTable.ToDictionary(x => x.Table, y => y.Chosen);
            // Check if tables have unique ranking
            var considerList = new List<DS_Cap>();
            var tables = matchParam.ChosenPerTable.Where(m => m.Playoff);
            foreach (var table in tables)
            {
                // Any pair has repeated ranking then returns null
                var pair = _context.DS_Caps.Include(m => m.DS_Bang)
                    .Where(m => m.ID_Trinh == model.ID_Trinh && !m.Phe_Duyet && !m.Xac_Nhan)
                    .FirstOrDefault(m => m.DS_Bang.Ten == table.Table && m.Xep_Hang == (table.Chosen + 1));
                considerList.Add(pair);
            }
            considerList = considerList.OrderByDescending(m => m.Tran_Thang).ThenByDescending(m => m.Hieu_so).ThenBy(m => m.Boc_Tham).ToList();
            considerList.RemoveRange(0, matchParam.PlayOff1);
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
            return result;
        }
        async Task<bool> Table_ToSpecialAsync(List<DS_Cap> playoff1, MatchGeneratorViewModel matchParam, RoundTabViewModel model)
        {
            bool result = false;
            var totalPair = matchParam.ChosenPerTable.Sum(m => m.Chosen) + matchParam.PlayOff1 + matchParam.PlayOff2;
            var straight2Special = matchParam.ChosenPerTable.ToDictionary(x => x.Table, y => y.Chosen);
            FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/Special1stRound.json");
            var placement = (await JsonSerializer.DeserializeAsync<List<Special1stRound>>(fileStream)).Find(m => m.PairNum == totalPair && m.TableNum == straight2Special.Count);
            fileStream.Dispose();
            // Reset all special rounds' value before proceeding
            result = Special_ResetResult(model.ID_Trinh);

            if (placement != null && result)
            {
                var allPairs = _context.DS_Caps.Include(m => m.DS_Bang)
                    .Where(m => m.ID_Trinh == model.ID_Trinh && !m.Phe_Duyet && !m.Xac_Nhan).ToList();
                var ds_cap = new List<DS_Cap>();

                // Generate straight to special list 
                foreach (var table in straight2Special.Keys)
                {
                    for (int i = 1; i <= straight2Special[table]; i++)
                    {
                        ds_cap.AddRange(allPairs.Where(m => m.DS_Bang.Ten == table && m.Xep_Hang == i));
                    }
                }
                // Add playoff 1 players if any - Chỉ Add Playoff vị trí đầu
                if (playoff1 !=null) ds_cap.AddRange(playoff1);
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
                    {   // Chỉ lựa các cặp play off ở vị trí đầu tiên trước
                        pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                        if (pair.Any()) { special1stRound[i].ID_Cap2 = pair.First().Id; }
                    }
                    else // Else => unique pair from template
                    {
                        pair = ds_cap.Where(m => m.DS_Bang.Ten == code[1] && m.Xep_Hang == (int)char.GetNumericValue(code[0]));
                        // Ranking is unique => Assign pair to match
                        // Ignore otherwise
                        if (pair.Any()) special1stRound[i].ID_Cap2 = pair.First().Id;
                    }
                    var columns2Save = new List<string> { "ID_Cap1", "ID_Cap2", "Chon_Cap_1", "Chon_Cap_2" };
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(special1stRound[i].Id, special1stRound[i], columns2Save).Succeeded;
                    if (!result) break;
                }
            }
            return result;
        }
        
        
        public async Task<IActionResult> Playoff_Update(RoundTabViewModel model)
        {
            // Find and update Result + Score for Matches
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
            //----------------  Bắt đầu cập nhật vào vòng đặc biệt
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
                    var winPairs = _context.DS_Caps.Include(m => m.DS_Bang).Where(m => winId.Contains(m.Id)).ToList();  // Các cặp playoff2
                    // Thêm các cặp Playoff 1 vào
                    FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/Files/Json/MatchGenParam.json");
                    var matchParam = (await JsonSerializer.DeserializeAsync<List<MatchGeneratorViewModel>>(fileStream)).Find(m => m.ID_Trinh == model.ID_Trinh);
                    fileStream.Dispose();
                    var playoff1 = Table_ToPlayoff1(matchParam, model);
                    if (playoff1!=null) { winPairs.AddRange(playoff1); }
                    int mlen = special1stRound[0].Chon_Cap_2.Length;
                    // Quét theo cột
                    int ok = 0;
                    int j = 1;
                    int mhang ;
                    char mbang ;
                    while (ok < special1stRound.Count && j < mlen)
                    {
                        for (int i = 0; i < special1stRound.Count; i++)
                        {
                            if (special1stRound[i].ID_Cap2==null) 
                            {
                                mhang= (int)char.GetNumericValue(special1stRound[i].Chon_Cap_2[0]);
                                mbang= special1stRound[i].Chon_Cap_2[j]; 
                                for (int k = 0; k < winPairs.Count; k++) 
                                {   
                                    if (winPairs[k].DS_Bang.Ten== mbang && winPairs[k].Xep_Hang== mhang)
                                    { special1stRound[i].ID_Cap2 = winPairs[k].Id;
                                        ok++;
                                        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(special1stRound[i].Id, special1stRound[i], new List<string> { "ID_Cap2" }).Succeeded;
                                        break;
                                    }
                                }
                            }
                        }
                        j++;
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
        public IActionResult Special_Update(List<DS_Tran> model)
        {
            // Find and update result for Matches
            bool result = false;
            var matches = model.Where(m => m.Ma_Vong <= 6).ToList();
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
                    result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "Kq_1", "Kq_2", "ID_Cap1", "ID_Cap2" }).Succeeded;
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
            return TabVMGenerator(model.FirstOrDefault().ID_Trinh, result, Tab.Special, "");
        }
        public IActionResult Special_Reset(int id)
        {
            bool result = Special_ResetResult(id);
            return TabVMGenerator(id, result, Tab.Table, "");
        }
                
        bool Special_ResetResult(int idTrinh)
        {
            bool result = false;
            // Get first special round
            var special1stRound = _context.DS_Trans.Where(m => m.Ma_Vong <= 6 && m.ID_Trinh == idTrinh).ToList();
            foreach (var match in special1stRound)
            {
                match.ID_Cap1 = match.ID_Cap2 = null;
                result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(match.Id, match, new List<string> { "ID_Cap1", "ID_Cap2" }).Succeeded;
                if (!result) break;
            }
            if (result) _context.SaveChanges();
            return result;
        }
        public IActionResult End_Score(int idTrinh)
        {
            new ScoreCalculation(_context).Player_PointDistribution(idTrinh);
            return TabVMGenerator(idTrinh,true, Tab.Point, "");
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

    }
}
