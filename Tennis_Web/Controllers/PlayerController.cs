using DataAccess;
using Library;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly TennisContext _context;
        public PlayerController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.DS_VDVs.OrderBy(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult Update(int? id)
        {
            var destination = new DatabaseMethod<DS_VDV>(_context).GetOjectFromDB(id);
            return View(destination);
        }
        [HttpPost]
        public IActionResult Update(int? id, DS_VDV source)
        {
            bool a1 = id == null && _context.DS_VDVs.Any(m => m.Ten_Tat == source.Ten_Tat);
            bool a2 = _context.DS_VDVs.Any(m => m.Ten_Tat == source.Ten_Tat && m.Id == id);
            if (a1 || a2)
            {
                ModelState.AddModelError(string.Empty, "Tên tắt bị trùng. Nhập tên mới !");
                return View(source);
            }
            var columnsToSave = new List<string> { "Ho", "Ten", "Ten_Tat","Gioi_Tinh", "CLB", "KhachMoi", "FileAnh", "Tel", "Email", "Status", "CongTy", "ChucVu" };
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, columnsToSave);
            _context.SaveChanges();
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
    }
}
