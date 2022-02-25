﻿using DataAccess;
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
    public class MatchController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public MatchController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult MatchInfo(TabViewModel model)
        {
            return View(model);
        }
        //public IActionResult UpdateResult(int id)
        //{
        //    var model = _context.DS_Trans.Include(m => m.DS_Cap1).Include(m => m.DS_Cap2).Where(m => m.Id == id).FirstOrDefault();
        //    if (model == null)
        //    {
        //        ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
        //    }
        //    else
        //    {
        //        var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model.ID_Cap1).FirstOrDefault();
        //        var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model.ID_Cap2).FirstOrDefault();
        //        model.DS_Cap1 = cap1;
        //        model.DS_Cap2 = cap2;
        //    }
        //    return PartialView(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateResultAsync(DS_Tran item)
        //{
        //    ResultModel<DS_Tran> result;
        //    if (item.Id != 0)
        //    {
        //        result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Kq_1", "Kq_2" });
        //        if (result.Succeeded)
        //        {
        //            await _context.SaveChangesAsync();
        //            var temp = _context.DS_Caps.Find(item.ID_Cap1);
        //            return RedirectToAction(nameof(Index), temp.ID_Trinh);
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, result.Message);
        //            return PartialView(item);
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
        //        return PartialView(item);
        //    }
        //}
        public IActionResult AdditionalInfo(int id)
        {
            ViewBag.TourName = _context.DS_Giais.Find(id).Ten;
            // Display for previous tournament
            // Get all levels from given tournament
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == id).Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            var pairs = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).ToList();
            var vdv1_Ids = pairs.Select(m => m.ID_Vdv1);
            var vdv2_Ids = pairs.Select(m => m.ID_Vdv2);
            // Get all players with from Player Id found in Player1 and Player2 lists
            var playersFromPair = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            // Get all players that haven't been put to any pair, or all pairs that haven't got a code
            var noPairPlayers = ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(playersFromPair).OrderByDescending(m => m.Diem).ThenByDescending(m => m.Diem_Cu).ToList();
            var noCodePairs = ViewBag.NoCodePairs = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh).Where(m => levels.Contains(m.ID_Trinh) && m.Ma_Cap == null).OrderBy(m => m.DS_Trinh.Trinh).ToList();
            // If there's any players who haven't been put in pairs, or pairs that haven't got a code, return error
            if (noCodePairs.Count > 0 || noPairPlayers.Count > 0)
            {
                ViewBag.DS_Trinh = _context.DS_Trinhs.Where(m => m.ID_Giai == id).OrderBy(m => m.Trinh);
                return View("Error"); 
            }
            // ---------------- If all conditions met ----------------
            var pair_Ids = pairs.Select(m => m.Id);
            // If any match found, return error message before proceeding
            if (_context.DS_Trans.Any(m => pair_Ids.Contains((int)m.ID_Cap1) || pair_Ids.Contains((int)m.ID_Cap2)))
            {
                ViewBag.Exist = true;
                ModelState.AddModelError(string.Empty, "Đã có danh sách trận trong cơ sở dữ liệu! Nếu tiếp tục sẽ xóa danh sách trận cũ.");
            }
                
            var model = new List<MatchGeneratorViewModel>();
            foreach (var level in levels)
            {
                var dict = new List<ChosenPerTable>();
                // Get name of all (distinct) tables from the given Ma_Cap (Pair Code)
                var tables = pairs.Where(m => m.ID_Trinh == level).GroupBy(m => char.ToUpper(m.Ma_Cap.ElementAt(0))).Select(m => new
                {
                    Table = m.Key,
                    Num = m.Count()
                }).OrderBy(m => m.Table).ToList();
                foreach (var table in tables)
                {
                    // Add table and default value (0) to list
                    dict.Add(new ChosenPerTable { Table = table.Table, PairsNum = table.Num, Chosen = 0 });
                }
                // Assign default values per Level
                var temp = new MatchGeneratorViewModel
                {
                    ID_Trinh = level,
                    Trinh = _context.DS_Trinhs.Find(level).Trinh,
                    ChosenPerTable = dict
                };
                // Add all levels to model (Tournament)
                model.Add(temp);
            }
            return View(model.OrderBy(m => m.Trinh).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AdditionalInfoAsync(List<MatchGeneratorViewModel> model, bool exist)
        {
            ViewBag.TourName = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == model.FirstOrDefault().ID_Trinh).FirstOrDefault().DS_Giai.Ten;
            if (exist)
            {
                ViewBag.Exist = true;
                ModelState.AddModelError(string.Empty, "Đã có danh sách trận trong cơ sở dữ liệu! Nếu tiếp tục sẽ xóa danh sách trận cũ.");
            }
            bool error = false;
            // Check for any errors before proceeding
            foreach (var level in model)
            {
                var tuple = PowerOfTwo(level.ChosenPerTable.Sum(m => m.Chosen) + level.PlayOff1 + level.PlayOff2);
                if (tuple.Item1 != 0)
                {
                    error = true;
                    if (tuple.Item1 < 0) ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " có tổng số cặp đi tiếp quá lớn hoặc quá nhỏ so với cho phép!");
                    else if (tuple.Item1 > 0) ModelState.AddModelError(string.Empty, "Thêm hoặc bớt cặp trình " + level.Trinh + " để tổng số cặp đi tiếp là " + tuple.Item1 + " hoặc " + tuple.Item2);
                }
                if (level.PlayOff1 != 0 && !level.ChosenPerTable.Any(m => m.P1))
                {
                    error = true;
                    ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " chưa chọn bảng cho Playoff 1!");
                }
                if (level.PlayOff2 != 0 && !level.ChosenPerTable.Any(m => m.P2))
                {
                    error = true;
                    ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " chưa chọn bảng cho Playoff 2!"); 
                }
            }
            if (error) return View(model);

            // Get rounds' Ids
            var rounds = _context.DS_Vongs.ToDictionary(x => x.Ma_Vong, y => y.Id);

            if (model != null)
            {
                // Get level ids
                var levels = model.Select(m => m.ID_Trinh).ToList();
                // Get all pairs with Level Id from the level id list
                var pairs = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).ToList();
                // ---------------- Update Table Id to pairs (Create new table if needed) ----------------
                foreach (var pair in pairs)
                {
                    var table = _context.DS_Bangs.Where(m => m.Ten == char.ToUpper(pair.Ma_Cap.ElementAt(0)) && m.ID_Trinh == pair.ID_Trinh).FirstOrDefault();
                    if (table == null)
                    {
                        _context.Add(new DS_Bang
                        {
                            Ten = char.ToUpper(pair.Ma_Cap.ElementAt(0)),
                            ID_Trinh = pair.ID_Trinh
                        });
                        _context.SaveChanges();
                        table = _context.DS_Bangs.OrderBy(m => m.Id).Last();
                    }
                    pair.ID_Bang = table.Id;
                    var result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "ID_Bang" });
                    if (!result.Succeeded) goto exit;
                }
                // ---------------- Save these paramters to Json file ----------------

                // Get path for the Json file
                string path = _webHost.WebRootPath + "/Files/Json/MatchGenParam.json";
                FileStream fileStream;
                // Delete file if Json file is already exist
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                fileStream = System.IO.File.Create(path);
                var options = new JsonSerializerOptions { WriteIndented = true };
                await JsonSerializer.SerializeAsync(fileStream, model, options);
                fileStream.Dispose();

                // ---------------- Delete old matches ----------------
                if (exist)
                {
                    // Get old matches
                    var oldMatches = _context.DS_Trans.Where(m => pairs.Select(m => m.Id).Contains((int)m.ID_Cap1) || pairs.Select(m => m.Id).Contains((int)m.ID_Cap2)).ToList();
                    var emptyMatched = _context.DS_Trans.Where(m => m.ID_Cap1 == null || m.ID_Cap2 == null).ToList();
                    oldMatches.AddRange(emptyMatched);
                    _context.RemoveRange(oldMatches);
                }
                // ---------------- Generate list of matches here ----------------

                foreach (var level in model)
                {
                    int count = 1;
                    string order = "";
                    // Add Table Rounds
                    foreach (var table in level.ChosenPerTable.Select(m => m.Table))
                    {
                        var pairByTable = pairs.Where(m => char.ToUpper(m.Ma_Cap.ElementAt(0)) == table && m.ID_Trinh == level.ID_Trinh).ToList();
                        for (int i = 0; i < pairByTable.Count; i++)
                        {
                            if (count < 10) order = "00" + count;
                            else if (count < 100) order = "0" + count;
                            else if (count < 1000) order = count.ToString();
                            var match = new DS_Tran
                            {
                                Ma_Tran = level.Trinh + "8" + table + order,
                                ID_Cap1 = pairByTable[i].Id,
                                ID_Vong = rounds[8]
                            };
                            // Match each pair with the next one
                            if (i == pairByTable.Count - 1) match.ID_Cap2 = pairByTable[0].Id;
                            // Unless it's the last pair, then pair with the first one
                            else match.ID_Cap2 = pairByTable[i + 1].Id;
                            _context.Add(match);
                            count++;
                        }
                    }
                    // Add playoff rounds (if any)
                    for (int i = 0; i < level.PlayOff2; i++)
                    {
                        if (count < 10) order = "00" + count;
                        else if (count < 100) order = "0" + count;
                        else if (count < 1000) order = count.ToString();
                        var match = new DS_Tran
                        {
                            Ma_Tran = level.Trinh + "7" + order,
                            ID_Vong = rounds[7]
                        };
                        _context.Add(match);
                        count++;
                    }
                    // Add Special Rounds
                    var totalPairs = level.ChosenPerTable.Sum(m => m.Chosen) + level.PlayOff1 + level.PlayOff2;
                    var ma_vong = (int)Math.Log2(totalPairs);
                    while (ma_vong > 0)
                    {
                        for (int i = totalPairs / 2; i > 0; i--)
                        {
                            if (count < 10) order = "00" + count;
                            else if (count < 100) order = "0" + count;
                            else if (count < 1000) order = count.ToString();
                            var match = new DS_Tran
                            {
                                Ma_Tran = level.Trinh + ma_vong.ToString() + order,
                                ID_Vong = rounds[ma_vong]
                            };
                            _context.Add(match);
                            count++;
                        }
                        ma_vong--;
                        totalPairs /= 2;
                    }
                }
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            exit:
            ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            return View(model);
        }

        // Get info from Json file
        // List<MatchGeneratorViewModel> model = JsonSerializer.Deserialize<List<MatchGeneratorViewModel>>(System.IO.File.ReadAllText(wwwRootPath + "/Files/Json/MatchGenParam.json"));

        public Tuple<int, int> PowerOfTwo(int number)
        {
            var powerOfTwo = new List<int> { 2, 4, 8, 16, 32, 64};
            // If number is a power of 2, return 0
            if (powerOfTwo.Contains(number)) return new Tuple<int, int>(0, 0);
            for (int i = 0; i < powerOfTwo.Count - 1; i++)
            {
                // Return the range number is in
                if (powerOfTwo[i] < number && number < powerOfTwo[i + 1])
                    return new Tuple<int, int>(powerOfTwo[i], powerOfTwo[i + 1]);
            }
            // if number is out of powerOfTwo range, return -1
            return new Tuple<int, int>(-1, -1);
        }
    }
}
