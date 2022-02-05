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
    public class MatchController : Controller
    {
        private readonly TennisContext _context;
        public MatchController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            var pairs = _context.DS_Caps.Where(m => m.ID_Trinh == id).Select(m => m.Id);
            var model = _context.DS_Trans.Where(m => pairs.Contains(m.ID_Cap1)).ToList();
            if (model != null)
            {
                for (int i = 0; i< model.Count; i++)
                {
                    var cap1 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap1).FirstOrDefault();
                    var cap2 = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Where(m => m.Id == model[i].ID_Cap2).FirstOrDefault();
                    // Assign Pair 1 and 2 info to list of matches
                    model[i].DS_Cap1 = cap1;
                    model[i].DS_Cap2 = cap2;
                    // Set bool value for Da_Nhap field
                    if (model[i].Kq_1 != 0 || model[i].Kq_2 != 0) model[i].Da_Nhap = true;
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
        public IActionResult UpdateResult(DS_Tran item)
        {
            ResultModel<DS_Tran> result;
            if (item.Id != 0)
            {
                result = new DatabaseMethod<DS_Tran>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Kq_1", "Kq_2" });
                if (result.Succeeded)
                {
                    _context.SaveChanges();
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
            var pair_Ids = pairs.Select(m => m.Id);
            // If any match found, return error message before proceeding
            if (_context.DS_Trans.Any(m => pair_Ids.Contains(m.ID_Cap1) || pair_Ids.Contains((int)m.ID_Cap2)))
                ModelState.AddModelError(string.Empty, "Đã có danh sách trận trong cơ sở dữ liệu! Nếu tiếp tục sẽ xóa danh sách trận cũ.");
            
            return View();
        }
    }
}
