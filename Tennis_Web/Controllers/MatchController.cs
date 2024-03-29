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
using AspNetCoreHero.ToastNotification.Abstractions;


namespace Tennis_Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public readonly INotyfService _notyf;

        public MatchController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        public IActionResult Index(bool isCurrent)
        {
            var success = (bool?) TempData["MatchList"] ;
            if (success == true) _notyf.Success("Đã tạo xong danh sách trận đấu");
            var model = _context.DS_Giais.Where(m => m.Giai_Moi == isCurrent).OrderByDescending(m => m.Ngay).ToList();
            ViewBag.isCurrent = isCurrent;
            return View(model);
        }
        public IActionResult IndexLevel(int id)
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.ID_Giai == id).OrderBy(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult MatchInfo(TabViewModel model)
        {
            if (TempData["Table"] !=null ) _notyf.Success(TempData["Table"].ToString());
            return View(model);
        }
        public async Task<IActionResult> AdditionalInfoAsync(int id)
        {
            ViewBag.TourName = _context.DS_Giais.Find(id).Ten;
            // Display for previous tournament
            // Get all levels from given tournament
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == id);
            var levelIds = levels.Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            var pairs = _context.DS_Caps.Where(m => levelIds.Contains(m.ID_Trinh) && !m.Phe_Duyet && !m.Xac_Nhan).ToList();
            var vdv_Ids = pairs.SelectMany(m => new[] { m.ID_Vdv1, m.ID_Vdv2 });
            // Get all players with from Player Id found in Player1 and Player2 lists
            var playersFromPair = _context.DS_VDVs.Where(m => vdv_Ids.Contains(m.Id));
            // Get all players that haven't been put to any pair, or all pairs that haven't got a code
            var noPairPlayers = ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(playersFromPair).OrderByDescending(m => m.Diem).ThenByDescending(m => m.Diem_Cu).ToList();
            var noCodePairs = ViewBag.NoCodePairs = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh)
                .Where(m => levelIds.Contains(m.ID_Trinh) && m.Ma_Cap == null && !m.Phe_Duyet).OrderBy(m => m.DS_Trinh.Trinh).ToList();
            // If there's any players who haven't been put in pairs, or pairs that haven't got a code, return error
            if (noCodePairs.Count > 0 || noPairPlayers.Count > 0)
            {
                ViewBag.DS_Trinh = _context.DS_Trinhs.Where(m => m.ID_Giai == id).OrderBy(m => m.Trinh);
                return View("Error");
            }

            // -------------------------------- If all conditions met --------------------------------

            // If any match found, return error message before proceeding
            if (_context.DS_Trans.Any(m => levelIds.Contains(m.ID_Trinh)))
            {
                ViewBag.Exist = true;
                ModelState.AddModelError(string.Empty, "Đã có danh sách trận trong cơ sở dữ liệu! Nếu tiếp tục sẽ xóa danh sách trận cũ.");
            }
            var model = new List<MatchGeneratorViewModel>();
            foreach (var level in levels)
            {
                var dict = new List<ChosenPerTable>();
                // Get name of all (distinct) tables from the given Ma_Cap (Pair Code)
                var tables = pairs.Where(m => m.ID_Trinh == level.Id).GroupBy(m => char.ToUpper(m.Ma_Cap[0])).Select(m => new
                {
                    Table = m.Key,
                    Num = m.Count()
                }).OrderBy(m => m.Table).ToList();
                foreach (var table in tables)
                {
                    // Add table and default value (0) to list
                    dict.Add(new ChosenPerTable { Table = table.Table, PairsNum = table.Num, Chosen = 0 });
                }
                // Add all levels to model (Tournament)
                model.Add(new MatchGeneratorViewModel
                {
                    ID_Trinh = level.Id,
                    Trinh = level.Trinh,
                    ChosenPerTable = dict
                });
            }
            model = model.OrderBy(m => m.Trinh).ToList();
            // -------------------  Đọc dữ liệu cũ ra nếu có để gán lại vào form dữ liệu mới
            FileStream fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/uploads/Json/MatchGenParam.json");
            //FileStream fileStream = System.IO.File.OpenRead("~/wwwroot/uploads/Json/MatchGenParam.json");
            var matchParam = (await JsonSerializer.DeserializeAsync<List<MatchGeneratorViewModel>>(fileStream)).ToList();
            fileStream.Dispose();
            for (int i=0; i<model.Count;i++) 
            { 
                if ((i<matchParam.Count) && (model[i].ID_Trinh==matchParam[i].ID_Trinh))
                {
                    model[i].PlayOff1 = matchParam[i].PlayOff1;
                    model[i].PlayOff2 = matchParam[i].PlayOff2;
                    for (int j=0; j< model[i].ChosenPerTable.Count; j++) 
                    { 
                        if ((j < matchParam[i].ChosenPerTable.Count) && (model[i].ChosenPerTable[j].Table == matchParam[i].ChosenPerTable[j].Table) )
                        {
                            model[i].ChosenPerTable[j].PairsNum = matchParam[i].ChosenPerTable[j].PairsNum;
                            model[i].ChosenPerTable[j].Chosen = matchParam[i].ChosenPerTable[j].Chosen;
                            model[i].ChosenPerTable[j].Playoff = matchParam[i].ChosenPerTable[j].Playoff;
                        }
                    }
                }
            }

            return View(model);
            //return View(model.OrderBy(m => m.Trinh).ToList());
        }
        /// <summary>
        /// 1. Xóa danh sách trận cũ nếu có 
        /// 2. Cập nhật lại danh sách các bảng
        /// 3. Trích điểm các cặp
        /// 4. Lưu lại tham số vào file JSON
        /// 5. Tạo danh sách trận đấu
        ///     - Danh sách trận vòng bảng
        ///     - Danh sách trận Playoff
        ///     - Danh sách trận vòng đặc biệt + Cập nhật Tham số vòng 1 đặc biệt
        /// </summary>
        /// <param name="model"></param>
        /// <param name="exist"></param>
        /// <returns></returns>
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
            //-------  Check for any errors before proceeding (Lỗi số cặp chọn, Playoff .v.v... không hợp lệ)
            foreach (var level in model)
            {
                var tuple = PowerOfTwo(level.ChosenPerTable.Sum(m => m.Chosen) + level.PlayOff1 + level.PlayOff2);
                if (tuple.Item1 != 0)
                {
                    error = true;
                    if (tuple.Item1 < 0) ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " có tổng số cặp đi tiếp quá lớn hoặc quá nhỏ so với cho phép!");
                    else if (tuple.Item1 > 0) ModelState.AddModelError(string.Empty, "Thêm hoặc bớt cặp trình " + level.Trinh + " để tổng số cặp đi tiếp là " + tuple.Item1 + " hoặc " + tuple.Item2);
                }
                // Bỏ trường hợp này --> do có thể khi xét vé vớt toàn cuch : chọn Playoff1 và loại 1 số cặp trực tiếp --> Còn lại 1 số cặp chẵn vào Playoff2
                //if ((level.PlayOff2>0) && ((level.PlayOff2 * 2 + level.PlayOff1) != level.ChosenPerTable.Count(m => m.Playoff)))
                //{
                //    error = true;
                //    var soBang = level.PlayOff2 * 2 + level.PlayOff1;
                //    ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " cần chọn " + soBang + " bảng cho Playoff!");
                //}
            }
            if (error) return View(model);

            if (model != null)
            {
                // Get level ids
                var levels = model.Select(m => m.ID_Trinh).ToList();
                // Get all pairs with Level Id from the level id list
                //var pairs = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh) && m.Phe_Duyet).ToList();
                var pairs = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).ToList();
                // ---------------- Delete old matches ----------------
                if (exist)
                {
                    // Get old matches
                    var oldMatches = _context.DS_Trans.Where(m => levels.Contains(m.ID_Trinh)).ToList();
                    _context.RemoveRange(oldMatches);
                }
                // ---------------- Update Table Id to pairs (Create new table if needed) ----------------
                // Reset old results from pairs
                foreach (var pair in pairs)
                {
                    var table = _context.DS_Bangs.FirstOrDefault(m => m.Ten == char.ToUpper(pair.Ma_Cap[0]) && m.ID_Trinh == pair.ID_Trinh);
                    if (table == null)
                    {
                        _context.Add(new DS_Bang
                        {
                            Ten = char.ToUpper(pair.Ma_Cap[0]),
                            ID_Trinh = pair.ID_Trinh
                        });
                        _context.SaveChanges();
                        table = _context.DS_Bangs.OrderBy(m => m.Id).Last();
                    }
                    pair.ID_Bang = table.Id;
                    pair.Diem = pair.Boc_Tham = pair.Xep_Hang = pair.Tran_Thang = pair.Hieu_so = 0;
                    var col2Save = new List<string> { "ID_Bang", "Diem", "Boc_Tham", "Xep_Hang", "Tran_Thang", "Hieu_so" };
                    var result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, col2Save);
                    if (!result.Succeeded) goto exit;
                }
                // ---------------- Trích điểm các cặp VĐV - Và tính tổng điểm trích để lưu vào file tham số ----------------
                for (int i=0; i<model.Count;i++)
                {
                    new ScoreCalculation(_context).Point_Deposit(model[i].ID_Trinh);
                }
                // ---------------- Save these paramters to Json file ----------------
                // Get path for the Json file
                string path = _webHost.WebRootPath + "/uploads/Json/MatchGenParam.json";
                //string path = "~/wwwroot/uploads/Json/MatchGenParam.json";
                FileStream fileStream;
                // Delete file if Json file is already exist
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                fileStream = System.IO.File.Create(path);
                var options = new JsonSerializerOptions { WriteIndented = true };
                await JsonSerializer.SerializeAsync(fileStream, model, options);
                fileStream.Dispose();

                // ---------------- Generate list of matches here ----------------

                foreach (var level in model)
                {
                    // Level(4) * Round(1) * Table(1) * Order round(2) * Order overall(3)
                    int count = 1;
                    string order = "";
                    // Add Table Rounds
                    foreach (var table in level.ChosenPerTable.Select(m => m.Table))
                    {
                        var pairByTable = pairs.Where(m => char.ToUpper(m.Ma_Cap[0]) == table && m.ID_Trinh == level.ID_Trinh).OrderBy(m => m.Ma_Cap).ToList();
                        for (int i = 0; i < pairByTable.Count - 1; i++)
                        {
                            for (int j = i + 1; j < pairByTable.Count; j++)
                            {
                                order = "*" + ("000" + count)[^3..];
                                _context.Add(new DS_Tran
                                {
                                    Ma_Tran = level.Trinh + "*8*" + table + "*00" + order,
                                    ID_Cap1 = pairByTable[i].Id,
                                    ID_Cap2 = pairByTable[j].Id,
                                    Ma_Vong = 8,
                                    ID_Trinh = level.ID_Trinh
                                });
                                count++;
                            }

                        }
                    }
                    // Add playoff rounds (if any)
                    for (int i = 0; i < level.PlayOff2; i++)
                    {
                        order = "*" + ("000" + count)[^3..];
                        _context.Add(new DS_Tran
                        {
                            Ma_Tran = level.Trinh + "*7*0*00" + order,
                            Ma_Vong = 7,
                            ID_Trinh = level.ID_Trinh
                        });
                        count++;
                    }
                    // Add Special Rounds
                    // Get position from template and add to first special round
                    var totalPairs = level.ChosenPerTable.Sum(m => m.Chosen) + level.PlayOff1 + level.PlayOff2;
                    fileStream = System.IO.File.OpenRead(_webHost.WebRootPath + "/uploads/Json/Special1stRound.json");
                    //fileStream = System.IO.File.OpenRead("~/wwwroot/uploads/Json/Special1stRound.json");
                    var placement = (await JsonSerializer.DeserializeAsync<List<Special1stRound>>(fileStream)).Find(m => m.PairNum == totalPairs && m.TableNum == level.ChosenPerTable.Count);
                    fileStream.Dispose();
                
                    int ma_vong = (int)Math.Log2(totalPairs), max_round = ma_vong;
                    string chon1 = "", chon2 = "";
                    while (ma_vong > 0)
                    {
                        int roundCount = 1;
                        string roundOrder = "";
                        for (int i = 0; i < totalPairs / 2; i++)
                        {
                            if (ma_vong == max_round && placement != null) 
                            {
                                // If position is found in template template => assign to first special round
                                chon1 = placement.P1_P2_Pair.ElementAt(i).Key;
                                chon2 = placement.P1_P2_Pair.ElementAt(i).Value;
                            }
                            order = "*" + ("000" + count)[^3..];
                            roundOrder = "*" + ("00" + roundCount)[^2..];
                            _context.Add(new DS_Tran
                            {
                                Ma_Tran = level.Trinh + "*" + ma_vong.ToString() + "*0" + roundOrder + order,
                                Ma_Vong = ma_vong,
                                ID_Trinh = level.ID_Trinh,
                                Chon_Cap_1 = chon1,
                                Chon_Cap_2 = chon2
                            });
                            roundCount++;
                            count++;
                        }
                        ma_vong--;
                        totalPairs /= 2;
                    }
                }
                TempData["MatchList"] = true;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { isCurrent = true });
            }
            exit:
            ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            return View(model);
        }

        public Tuple<int, int> PowerOfTwo(int number)
        {
            var powerOfTwo = new List<int> { 2, 4, 8, 16, 32, 64 };
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
