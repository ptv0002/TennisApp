using AspNetCoreHero.ToastNotification.Abstractions;
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
using System.Reflection;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;

namespace Tennis_Web.Areas.NoRole.Controllers
{
    [Area("NoRole")]
    [Route("[Action]")]
    public class PlayerAreaController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly INotyfService _notyf;
        public PlayerAreaController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        public IActionResult Player(bool isCurrent, bool? isGuest, bool participate)
        {
            bool? player = (bool?)TempData["SuccessfulUpdatePlayer"];
            if (player == true) _notyf.Success(TempData["Message"].ToString() ?? "Lưu thay đổi thành công!");

            List<DS_VDV> model = new();
            if (isCurrent)
            {
                model = _context.DS_VDVs.Where(m => m.Tham_Gia == participate).OrderByDescending(m => m.Diem).ToList();
                ViewBag.IsCurrent = true;
                ViewBag.Title = participate ? "VĐV tham gia" : "Đăng kí tham gia";
                ViewBag.Info = participate;
                ViewBag.Tournament = _context.DS_Giais.FirstOrDefault(m => m.Giai_Moi).Ten;
                // If register tab is active, display error or success message after registering
                if (!participate)
                {
                    bool? success = (bool?)TempData["SuccessfulRegister"];
                    if (success == true)
                    {
                        _notyf.Success("Đã đăng kí thành công - Chờ BTC phê duyệt");
                    }
                    else if (success == false) _notyf.Error("Có lỗi xảy ra khi đang đăng kí!");
                }
            }
            else
            {
                if (isGuest != null) { model = _context.DS_VDVs.OrderByDescending(m => m.Diem).Where(m => m.Khach_Moi == isGuest).ToList();}
                else { model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList(); }
                ViewBag.Title = isGuest == true ? "Hội viên khách mời" : (isGuest == false ? "Hội viên chính thức" : "Tất cả hội viên");
                ViewBag.Info = true;
                ViewBag.IsCurrent = false;

                bool? chkPw = (bool?)TempData["CheckPassword"];
                if (chkPw == false) { _notyf.Error(TempData["Message"].ToString() ?? "Có lỗi xảy ra khi đang lưu thay đổi!", 30); }

            }
            return View(model);
        }
        public IActionResult UpdatePlayer(int id)
        {
            bool? success = (bool?)TempData["SuccessfulPlayer"];
            if (success == true) _notyf.Success(TempData["Message"].ToString() ?? "Lưu thay đổi thành công!");
            else if (success == false) _notyf.Error(TempData["Message"].ToString() ?? "Có lỗi xảy ra khi đang lưu thay đổi!");
            
            var source = _context.DS_VDVs.Find(id);
            if (source != null && source.File_Anh != null) _notyf.Warning("Upload ảnh mới sẽ xóa ảnh cũ!", 100);
            var destination = new PasswordViewModel();
            if (source != null)
            {
                destination.Ho_Ten = source.Ho_Ten;
                destination.Ten_Tat = source.Ten_Tat;
                destination.Gioi_Tinh = source.Gioi_Tinh;
                destination.Tel = source.Tel;
                destination.CLB = source.CLB;
                destination.File_Anh = source.File_Anh;
                destination.Email = source.Email;
                destination.Cong_Ty = source.Cong_Ty;
                destination.Chuc_Vu = source.Chuc_Vu;
                destination.Diem = source.Diem;
                destination.Diem_Cu = source.Diem_Cu;
            }
            return View(destination);
        }
        [HttpPost]
        public IActionResult UpdatePlayer(int id, PasswordViewModel source)
        {
            if (source.Picture != null)
            {
                // Handle picture attachment
                string extension = Path.GetExtension(source.Picture.FileName);
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    if (source.Picture.Length <= 250000)
                    {
                        new FileMethod(_context, _webHost).SaveImage(source);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "File ảnh lớn hơn độ lớn cho phép! Upload ảnh nhỏ hơn 250 KB");
                        return View(source);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Dạng file " + extension + " không được hỗ trợ!");
                    return View(source);
                }
            }

            // Handle saving object
            var columnsToSave = new List<string> { "Ho_Ten", "Gioi_Tinh", "CLB", "File_Anh", "Tel", "Email", "Cong_Ty", "Chuc_Vu" };
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, columnsToSave);
            
            if (result.Succeeded)
            {
                _context.SaveChanges();
                TempData["SuccessfulUpdatePlayer"] = true;
                return RedirectToAction(nameof(Player));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
        public IActionResult DeleteImage(string id)
        {
            new FileMethod(_context, _webHost).DeleteImage(id);
            return RedirectToAction(nameof(UpdatePlayer), new { id = Convert.ToInt32(id) });
        }
        [HttpPost]
        public IActionResult ResetPassword(PasswordViewModel source)
        {
            if (ModelState.IsValid)
            {
                var destination = _context.DS_VDVs.Find(source.Id);
                destination.Password = source.NewPassword;
                
                var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(source.Id, destination, new List<string> { "Password" }).Succeeded;
                if (result) _context.SaveChanges();
                TempData["SuccessfulPlayer"] = true;
                TempData["Message"] = "Lưu mật khẩu mới thành công";
            }
            else
            {
                TempData["SuccessfulPlayer"] = false;
                TempData["Message"] = "Xác nhận mật khẩu không trùng khớp!";
            }
            return RedirectToAction(nameof(UpdatePlayer), new { id = source.Id });
        }
        public IActionResult History(int id)
        {
            var matches = _context.DS_Trans.Include(m => m.DS_Cap1.VDV1).Include(m => m.DS_Cap1.VDV2)
                .Include(m => m.DS_Cap2.VDV1).Include(m => m.DS_Cap2.VDV2)
                .Include(m => m.DS_Trinh.DS_Giai).Include(m => m.DS_Trinh)
                .Where(m => m.DS_Cap1.VDV1.Id == id || m.DS_Cap1.VDV2.Id == id || m.DS_Cap2.VDV1.Id == id || m.DS_Cap2.VDV2.Id == id)
                .OrderByDescending(m => m.DS_Trinh.DS_Giai.Ngay).ThenByDescending(m => m.Ma_Vong)
                .ToList();

            var DS_VDVDiem = _context.DS_VDVDiems.Where(m => m.ID_Vdv == id).OrderBy(m => m.Ngay).Select(m => new
            Extended_VDVDiem
            {
                Id = m.Id,
                ID_Trinh = m.ID_Trinh,
                ID_Vdv = m.ID_Vdv,
                Diem = m.Diem,
                Ngay = m.Ngay,
                Ghi_Chu = m.Ghi_Chu,
                Diem_Moi = 0,
                Diem_Cu = 0
            }).ToList();
            DS_VDVDiem[0].Diem_Moi = DS_VDVDiem[0].Diem_Cu + DS_VDVDiem[0].Diem;
            for (int i = 1; i < DS_VDVDiem.Count; i++)
            {
                DS_VDVDiem[i].Diem_Cu = DS_VDVDiem[i - 1].Diem_Moi;
                DS_VDVDiem[i].Diem_Moi = DS_VDVDiem[i].Diem_Cu + DS_VDVDiem[i].Diem;
            }

            return View(new PlayerHistoryViewModel
            {
                VDV = _context.DS_VDVs.Find(id),
                DS_Tran = matches,
                DS_VDVDiem = DS_VDVDiem.OrderByDescending(m => m.Ngay).ToList()
            });
        }
    }
}
