using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;
using Tennis_Web.Models;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class CurrentTabController : Controller
    {
        private readonly TennisContext _context;
        public readonly INotyfService _notyf;
        public CurrentTabController(TennisContext context,INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public List<DS_Cap> PopulateAutoComplete(int idVdv, IEnumerable<int> eligibleList)
        {
            var levels = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi);
            // Get all pairs with Level Id from the level id list
            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Select(m => m.Id).Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Select(m => m.Id).Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            // Get all players with from Player Id found in Player1 and Player2 lists
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            var eligiblePartner = _context.DS_VDVs.Where(m => m.Tham_Gia && m.Id != idVdv).Except(players).ToList();
            var model = new List<DS_Cap>();
            var info = _context.DS_VDVs.Find(idVdv);
            foreach (var partner in eligiblePartner)
            {
                var eligibleLevels = levels.Where(m => eligibleList.Contains(m.Id) &&
                    m.Trinh - m.BD_Duoi <= (info.Diem + partner.Diem) 
                    && (info.Diem + partner.Diem) <= m.Trinh + m.BD_Tren).ToList();
                if (eligibleLevels.Any())
                {
                    foreach (var level in eligibleLevels)
                    {
                        model.Add(new DS_Cap
                        {
                            Diem = info.Diem + partner.Diem,
                            ID_Vdv2 = partner.Id,
                            VDV2 = partner,
                            ID_Trinh = level.Id,
                        });
                    }                    
                }
                //else
                //{
                //    // Không có VĐV nào tương ứng để đăng ký - vui lòng chờ bốc thăm
                //    _notyf.Error("Không có VĐV đủ điều kiện đăng ký trước - Vui lòng chờ Bốc thăm sau !",30);}
            }
            return model;
        }
        public IActionResult UpdatePair(int id)
        {
            //var list = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh)
            //    .Where(m => m.ID_Vdv2 == id && m.Phe_Duyet && !m.Xac_Nhan)
            //    .OrderBy(m => m.DS_Trinh.Trinh).ThenByDescending(m => m.Diem);
            var model = _context.DS_Caps.Include(m => m.VDV1).Include(m => m.VDV2).Include(m => m.DS_Trinh)
                  .FirstOrDefault(m => m.ID_Vdv2 == id && m.Phe_Duyet && !m.Xac_Nhan);
            if (model == null)
            {
                var vdv = _context.DS_VDVs.Find(id);
                var levels = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.DS_Giai.Giai_Moi).OrderBy(m => m.Trinh);
                var eligible = new List<DS_Trinh>();
                foreach (var level in levels)
                {
                    bool a1 = level.Min_Point == 0;
                    bool a2 = level.Max_Point == 0;
                    switch (a1, a2)
                    {
                        case (true,true):
                            eligible.Add(level);
                            break;
                        case (true, false):
                            if (level.Max_Point > vdv.Diem) eligible.Add(level);
                            break;
                        case (false, true):
                            if (level.Min_Point < vdv.Diem) eligible.Add(level);
                            break;
                        case (false, false):
                            if (level.Min_Point < vdv.Diem && vdv.Diem < level.Max_Point) eligible.Add(level);
                            break;

                    }
                }
                //ViewBag.DS_Trinh = new SelectList(levels, "Id", "Trinh");
                ViewBag.LevelList = eligible;
                ViewBag.DS_VDV = PopulateAutoComplete(id, eligible.Select(m=>m.Id));
                model = new DS_Cap { ID_Vdv1 = id, VDV1 = _context.DS_VDVs.Find(id) };
            }
            //else { ViewBag.List = list; }
            return View(model);
        }
        [HttpPost]
        public IActionResult UpdatePair(DS_Cap pair)
        {
            var result = false;
            // New pair register
            if (pair.Id == 0)
            {
                var p1 = _context.DS_VDVs.Find(pair.ID_Vdv1);
                //pair.Id = 0;
                pair.Diem = p1.Diem + _context.DS_VDVs.Find(pair.ID_Vdv2).Diem;
                var level = _context.DS_Trinhs.Where(m => m.DS_Giai.Giai_Moi)
                    .FirstOrDefault(m => m.Trinh - m.BD_Duoi <= pair.Diem && pair.Diem <= m.Trinh + m.BD_Tren);
                pair.Phe_Duyet = true;
                pair.Xac_Nhan = false;
                pair.ID_Trinh = level.Id;
                pair.Ngay = DateTime.Now;
                result = true;
                _context.Add(pair);
                TempData["Message"] = "Đăng ký cặp thành công - Chờ partner xác nhận!";
            }
            // Confirm part form
            else
            {
                pair.Xac_Nhan = true;
                pair.Phe_Duyet = true;
                result = new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xac_Nhan", "Phe_Duyet" }).Succeeded;
                TempData["Message"] = "Cặp đấu đã đăng ký xong - Chờ BTC phê duyệt!";
            }
            if (result)
            {
                _context.SaveChanges();
                TempData["SuccessfulPair"] = true;
            }

            return RedirectToAction(nameof(Pair));
        }
        //public IActionResult AcceptPair(int id, int idVdv)
        //{
        //    var pair = _context.DS_Caps.Find(id);
        //    pair.Xac_Nhan = true;
        //    pair.Phe_Duyet = true;
        //    new DatabaseMethod<DS_Cap>(_context).SaveObjectToDB(pair.Id, pair, new List<string> { "Xac_Nhan", "Phe_Duyet" });
            
        //    var list = _context.DS_Caps.Include(m => m.DS_Trinh)
        //        .Where(m => m.ID_Vdv2 == idVdv && m.Phe_Duyet && !m.Xac_Nhan && m.Id != id);
        //    _context.RemoveRange(list);
        //    _context.SaveChanges();
        //    TempData["SuccessfulPair"] = true;
        //    TempData["Message"] = "Cặp đấu đã đăng ký xong - Chờ BTC phê duyệt!";
        //    return RedirectToAction(nameof(Pair));
        //}
        public IActionResult DeletePair(int id)
        {
            var pair = _context.DS_Caps.Include(m => m.VDV1).FirstOrDefault(m => m.Id == id);
            var name = pair.VDV1.Ten_Tat;
            _context.Remove(pair);
            _context.SaveChanges();
            TempData["WarningPair"] = true;
            TempData["Message"] = "Không đồng ý đứng chung với " + name + "!";
            return RedirectToAction(nameof(Pair));
        }
        public IActionResult Pair(string selected)
        {
            bool? success = (bool?)TempData["SuccessfulPair"];
            bool? chkPw = (bool?)TempData["CheckPassword"];
            bool? warning = (bool?)TempData["WarningPair"];

            if (chkPw == false) { _notyf.Error((string)(TempData["Message"] ?? "Có lỗi xảy ra khi đang lưu thay đổi!")); }
            if (success == true) { _notyf.Success((string)(TempData["Message"] ?? "Lưu thay đổi thành công!")); }
            if (warning == true) { _notyf.Warning((string)TempData["Message"]); }
            
            var current = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi);
            var model = _context.DS_Caps.Include(m => m.DS_Trinh).Include(m => m.VDV1).Include(m => m.VDV2)
                .Where(m => m.DS_Trinh.ID_Giai == current.Id)
                .OrderBy(m => m.DS_Trinh.Trinh).ThenBy(m => m.Ma_Cap).ToList();


            string errorMsg = "";

            switch (selected)
            {
                case "1": // Cặp đã được phê duyệt (thành công)
                    model = model.Where(m => !m.Phe_Duyet && !m.Xac_Nhan).ToList();
                    ViewBag.Type = 1;
                    errorMsg = "Không có cặp đã được phê duyệt";
                    break;
                case "2": // Cặp thiếu chữ kí xác nhận
                    model = model.Where(m => m.Phe_Duyet && !m.Xac_Nhan).ToList();
                    ViewBag.Type = 2;
                    errorMsg = "Không có cặp thiếu chữ kí xác nhận";
                    break;
                case "3": // Cặp chờ phê duyệt
                    model = model.Where(m => m.Phe_Duyet && m.Xac_Nhan).ToList();
                    ViewBag.Type = 3;
                    errorMsg = "Không có cặp chờ phê duyệt";
                    break;
                default:
                    ViewBag.Type = 0;
                    errorMsg = "Không có danh sách cặp";
                    break;
            }
            if (!model.Any()) { _notyf.Error(errorMsg); }
            var list = model.GroupBy(m => m.DS_Trinh.Trinh).Select(m => new
            {
                Trinh = m.Key,
                Num = m.Count()
            }).OrderBy(m => m.Trinh);
            ViewBag.ListLevel = list.Select(m => m.Trinh).ToList();
            ViewBag.ListNum = list.Select(m => m.Num).ToList(); 
            // Generate List of all participated players with no pairs
            var levels = _context.DS_Trinhs.Where(m => m.ID_Giai == current.Id).Select(m => m.Id);
            // Get all pairs with Level Id from the level id list
            var vdv1_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv1);
            var vdv2_Ids = _context.DS_Caps.Where(m => levels.Contains(m.ID_Trinh)).Select(m => m.ID_Vdv2);
            // Get all players with from Player Id found in Player1 and Player2 lists
            var players = _context.DS_VDVs.Where(m => vdv1_Ids.Contains(m.Id) || vdv2_Ids.Contains(m.Id));
            ViewBag.NoPairPlayers = _context.DS_VDVs.Where(m => m.Tham_Gia).Except(players).OrderByDescending(m => m.Diem).ToList();
            ViewBag.Tournament = current.Ten;
            return View(model);
        }
        public IActionResult Register(int id)
        {
            var item = new DS_VDV
            {
                Id = id,
                Phe_Duyet = true
            };
            bool result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(item.Id, item, new List<string> { "Phe_Duyet" }).Succeeded;
            TempData["SuccessfulRegister"] = result;
            if (result) { _context.SaveChanges(); }
            return RedirectToAction("Player", "PlayerArea", new { isCurrent = true, participate = false });
        }
        public IActionResult ResultInfo(ResultViewModel model)
        {
            return View(model);
        }
        public IActionResult SwitchToTabs(string tabname, bool isCurrent, int id)
        {
            var level = _context.DS_Trinhs.Include(m => m.DS_Giai).Where(m => m.Id == id).FirstOrDefault();
            var vm = new ResultViewModel()
            {
                IsCurrent = isCurrent,
                ID = id,
                Level = level.Trinh.ToString(),
                Tournament = level.DS_Giai.Ten
            };
            switch (tabname)
            {
                case "Table":
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
                case "Special":
                    vm.ActiveTab = Tab.Special;
                    return RedirectToAction("ResultInfo", vm);
                case "Point":
                    vm.ActiveTab = Tab.Point;
                    return RedirectToAction("ResultInfo", vm);
                default:
                    vm.ActiveTab = Tab.Table;
                    return RedirectToAction("ResultInfo", vm);
            }
        }
    }
}
