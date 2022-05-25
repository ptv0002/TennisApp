using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Areas.NoRole.Models;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public readonly INotyfService _notyf;
        public PlayerController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            bool? success = (bool?)TempData["SuccessfulUpdatePlayer"];
            if (success == true) { _notyf.Success("Lưu thay đổi thành công!"); }
            else if (success == false) { _notyf.Error("Có lỗi xảy ra khi đang thay đổi!"); }
            var model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult UpScore()
        {
            new ScoreCalculation(_context).Player_Update();
            TempData["SuccessfulUpdatePlayer"] = true;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            bool? success = (bool?)TempData["SuccessfulScore"];
            if (success == true) {_notyf.Success("Lưu thay đổi thành công!");}
            var destination = _context.DS_VDVs.Find(id);
            if (destination != null && destination.File_Anh != null) _notyf.Warning("Upload ảnh mới sẽ xóa ảnh cũ!", 100);
            return View(destination);
        }
        [HttpPost]
        public IActionResult Update(int id, DS_VDV source)
        {
            bool a = _context.DS_VDVs.Any(m => m.Ten_Tat.ToUpper() == source.Ten_Tat.ToUpper() && (m.Id != id || id == 0));
            if (a)
            {
                ModelState.AddModelError(string.Empty, "Tên tắt bị trùng. Nhập tên mới!");
                return View(source);
            }
            if (source.Picture != null)
            {
                // Handle picture attachment
                string extension = Path.GetExtension(source.Picture.FileName);
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    if (source.Picture.Length <= 1000000)
                    {
                        new FileMethod(_context, _webHost).SaveImage(source);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "File ảnh lớn hơn độ lớn cho phép! Upload ảnh nhỏ hơn 1MB");
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
            var columnsToSave = new List<string> { "Ho_Ten", "Ten_Tat", "Gioi_Tinh", "CLB", "Khach_Moi", "File_Anh", "Tel", "Email", "Status", "Cong_Ty", "Chuc_Vu" };
            if (id == 0)
            {
                columnsToSave.Add("Diem");
                columnsToSave.Add("Password");
                source.Password = "bitkhanhhoa@newuser";
            }
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, columnsToSave);
            if (result.Succeeded) { _context.SaveChanges(); }

            if (id == 0)
            {   // --  Cập nhật điểm VĐV mới nhập vào phần phát sinh - Điều chỉnh nhân viên không cho sửa điểm
                var mid = _context.DS_VDVs.OrderBy(m => m.Id).Last().Id;
                var mDiemPS = new DS_VDVDiem() { ID_Vdv = mid, Ngay = DateTime.Now, Diem = source.Diem, Ghi_Chu = "" };
                _context.Add(mDiemPS);
            }
            if (result.Succeeded)
            {
                _context.SaveChanges();
                TempData["SuccessfulUpdatePlayer"] = true;
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
        public IActionResult DeleteImage(string id)
        {
            new FileMethod(_context, _webHost).DeleteImage(id);
            return RedirectToAction(nameof(Update), new { id = Convert.ToInt32(id) });
        }
        public IActionResult ResetPassword(string id)
        {
            var source = new DS_VDV { Password = "bitkhanhhoa@newuser"};
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(Convert.ToInt32(id), source, new List<string> { "Password" });
            if (result.Succeeded)
            {
                _context.SaveChanges();
                TempData["SuccessfulUpdatePlayer"] = true;
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult ScoreUpdate(int idVDV)
        {
            var player = _context.DS_VDVs.Find(idVDV);
            var model = new Extended_VDVDiem()
            {
                ID_Vdv = player.Id,
                DS_VDV = player,
                Diem_Cu = player.Diem_Cu,
                Diem_Moi = player.Diem,
                Ngay = DateTime.Now
            };
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ScoreUpdate(Extended_VDVDiem model)
        {
            // Successfully update
            TempData["SuccessfulScore"] = true;
            return RedirectToAction(nameof(Update), new { id = model.ID_Vdv });
        }
    }
}
