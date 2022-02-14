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
    public class MatchController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public MatchController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index(int id)
        {
            var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == id).Select(m => m.Id);
            var model = _context.DS_Trans.Where(m => pairs.Contains((int)m.ID_Cap1) || pairs.Contains((int)m.ID_Cap2)).ToList();
            if (model != null)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap1).FirstOrDefault();
                    var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap2).FirstOrDefault();
                    // Assign Pair 1 and 2 info to list of matches
                    model[i].DS_Cap1 = cap1;
                    model[i].DS_Cap2 = cap2;
                }
            }
            var temp = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == id).FirstOrDefault();
            ViewBag.DetailedTitle = "giải " + temp.DS_Giai.Ten + " - Trình " + temp.Trinh;
            return View(model);
        }
        public IActionResult TournamentLevel()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult UpdateResult(int id)
        {
            var model = _context.DS_Trans.Include(m => m.DS_Cap1).Include(m => m.DS_Cap2).Where(m => m.Id == id).FirstOrDefault();
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            }
            else
            {
                var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model.ID_Cap1).FirstOrDefault();
                var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model.ID_Cap2).FirstOrDefault();
                model.DS_Cap1 = cap1;
                model.DS_Cap2 = cap2;
            }
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateResultAsync(DS_Tran item)
        {
            ResultModel<DS_Tran> result;
            if (item.Id != 0)
            {
                result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Kq_1", "Kq_2" });
                if (result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    var temp = _context.DS_Caps.Find(item.ID_Cap1);
                    return RedirectToAction(nameof(Index), temp.ID_Trinh);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return PartialView(item);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
                return PartialView(item);
            }
        }
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
            var playersFromDB = _context.DS_VDVs.Where(m => m.Tham_Gia);
            // Get all players that haven't been put to any pair, or all pairs that haven't got a code
            var noPairPlayers = playersFromDB.Except(playersFromPair).OrderByDescending(m => m.Diem).ThenByDescending(m => m.Diem_Cu);
            var noCodePairs = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh).Where(m => levels.Contains(m.ID_Trinh) && m.Ma_Cap == null).OrderBy(m => m.DS_Trinh.Trinh);
            // If there's any players who haven't been put in pairs, or pairs that haven't got a code, return error
            if (noCodePairs.Any() || noPairPlayers.Any())
            {
                ViewBag.DS_Trinh = _context.DS_Trinhs.Where(m => m.ID_Giai == id).OrderBy(m => m.Trinh);
                return View("Error", new MatchGeneratorErrorViewModel
                {
                    NoCodePairs = noCodePairs,
                    NoPairPlayers = noPairPlayers
                });
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
                var dict = new List<NumPerTable>();
                // Get name of all (distinct) tables from the given Ma_Cap (Pair Code)
                var tables = pairs.Where(m => m.ID_Trinh == level).Select(m => char.ToUpper(m.Ma_Cap.ElementAt(0))).Distinct();
                foreach (var table in tables)
                {
                    // Add table and default value (0) to list
                    dict.Add(new NumPerTable { Table = table, Num = 0 });
                }
                // Assign default values per Level
                var temp = new MatchGeneratorViewModel
                {
                    ID_Trinh = level,
                    Trinh = _context.DS_Trinhs.Find(level).Trinh,
                    NumPerTable = dict
                };
                // Add all levels to model (Tournament)
                model.Add(temp);
            }
            return View(model);
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

            foreach (var level in model)
            {
                var tuple = PowerOfTwo(level.NumPerTable.Sum(m => m.Num) + level.PlayOff1 + level.PlayOff2);
                if (tuple.Item1 < 0)
                {
                    ModelState.AddModelError(string.Empty, "Trình " + level.Trinh + " có tổng số cặp đi tiếp quá lớn hoặc quá nhỏ so với cho phép!");
                    return View(model);
                }
                else if (tuple.Item1 > 0)
                {
                    ModelState.AddModelError(string.Empty, "Thêm hoặc bớt cặp trình " + level.Trinh + " để tổng số cặp đi tiếp là " + tuple.Item1 + " hoặc " + tuple.Item2);
                    return View(model);
                }
            }
            if (model != null)
            {
                // ---------------- Save these paramters to Json file ----------------

                // Get path for the Json file
                string path = _webHost.WebRootPath + "/Files/Json/MatchGenParam.json";
                FileStream fileStream;
                // Delete file if Json file is already exist
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                fileStream = System.IO.File.Create(path);
                var options = new JsonSerializerOptions { WriteIndented = true };
                await JsonSerializer.SerializeAsync(fileStream, model, options);
                await fileStream.DisposeAsync();

                // ---------------- Delete old matches ----------------
                if (exist)
                {
                    // Get level ids
                    var levels = model.Select(m => m.ID_Trinh).ToList();
                    // Get all pairs with Level Id from the level id list
                    var pairs = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.Id).ToList();
                    // Get old matches
                    var oldMatches = _context.DS_Trans.Where(m => pairs.Contains((int)m.ID_Cap1) || pairs.Contains((int)m.ID_Cap2));
                    _context.RemoveRange(oldMatches);
                }
                // ---------------- Generate list of matches here ----------------
                var rounds = _context.DS_Vongs.ToDictionary(x => x.Ma_Vong, y => y.Id);

                foreach (var level in model)
                {
                    int count = 1;
                    // Add Table Rounds
                    foreach (var table in level.NumPerTable.Select(m => m.Table))
                    {
                        var pairByTable = _context.DS_Caps.Where(m => char.ToUpper(m.Ma_Cap.ElementAt(0)) == table && m.ID_Trinh == level.ID_Trinh).ToList();
                        for (int i = 0; i < pairByTable.Count; i++)
                        {
                            var match = new DS_Tran
                            {
                                Ma_Tran = level.Trinh + "9" + table + count,
                                ID_Cap1 = pairByTable[i].Id,
                                ID_Vong = rounds[9]
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
                        var match = new DS_Tran
                        {
                            Ma_Tran = level.Trinh + "8" + count,
                            ID_Vong = rounds[8]
                        };
                        _context.Add(match);
                        count++;
                    }
                    // Add Special Rounds
                    var specMatches = (int)Math.Log2(level.NumPerTable.Sum(m => m.Num) + level.PlayOff1 + level.PlayOff2);
                    for (int i = specMatches; i > 0; i--)
                    {
                        var match = new DS_Tran
                        {
                            Ma_Tran = level.Trinh + i.ToString() + count,
                            ID_Vong = rounds[i]
                        };
                        _context.Add(match);
                        count++;
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TournamentLevel));
            }
            ModelState.AddModelError(string.Empty, "Lỗi hệ thống!");
            return View(model);
        }

        // Get info from Json file
        // List<MatchGeneratorViewModel> model = JsonSerializer.Deserialize<List<MatchGeneratorViewModel>>(System.IO.File.ReadAllText(wwwRootPath + "/Files/Json/MatchGenParam.json"));



        public Tuple<int, int> PowerOfTwo(int number)
        {
            var powerOfTwo = new List<int> { 2, 4, 8, 16, 32, 64, 128 };
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
        public List<Tuple<DS_Cap, int, int>> Ranking(List<DS_Cap> list)
        {
            // Get pair Ids
            var idCap = list.Select(m => m.Id).ToList();
            var rankedList = new List<Tuple<DS_Cap, int, int>>(); // T1: Pair, T2: Score, T3: Difference
            
            foreach (var pair in list)
            {
                int count = 0, indif = 0, dif = 0;

                // Get matches that the pair participate
                var matches = _context.DS_Trans.Where(m => m.ID_Cap1 == pair.Id || m.ID_Cap2 == pair.Id);
                foreach (var match in matches)
                {
                    if (match.ID_Cap1 == pair.Id) indif = match.Kq_1 - match.Kq_2;
                    else indif = match.Kq_2 - match.Kq_1;
                    if (indif > 0)
                    {
                        // If the pair wins, increment win count 
                        count++;
                    }
                    // Calculate the point difference
                    dif += indif;
                }
                var tuple = new Tuple<DS_Cap, int, int>(pair, count, dif);
                rankedList.Add(tuple);
            }
            // Order by the win score, then the point difference
            return rankedList.OrderByDescending(m => m.Item2).ThenByDescending(m => m.Item3).ToList();
        }
    }
}
