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
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class PlayerController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        public PlayerController(TennisContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult Update(int id)
        {
            var destination = _context.DS_VDVs.Find(id);
            return View(destination);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(int id, DS_VDV source)
        {
            bool a = _context.DS_VDVs.Any(m => m.Ten_Tat == source.Ten_Tat && (m.Id != id || id == 0));
            if (a)
            {
                ModelState.AddModelError(string.Empty, "Tên tắt bị trùng. Nhập tên mới!");
                return View(source);
            }
            if (source.Picture != null)
            {
                // Handle picture attachment
                string extension = Path.GetExtension(source.Picture.FileName);
                if(extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                {
                    // Save image to wwwroot/PlayerImg
                    string wwwRootPath = _webHost.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(source.Picture.FileName);
                    source.File_Anh = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/PlayerImg/", fileName);
                    using var fileStream = new FileStream(path, FileMode.Create);
                    await source.Picture.CopyToAsync(fileStream);
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "Dạng file " + extension + " không được hỗ trợ!");
                    return View(source);
                }
            }
            
            // Handle saving object
            var columnsToSave = new List<string> { "Ho", "Ten", "Ten_Tat", "Gioi_Tinh", "CLB", "Khach_Moi", "File_Anh", "Tel", "Email", "Status", "Cong_Ty", "Chuc_Vu" };
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, columnsToSave);
            if (result.Succeeded) 
            { 
                _context.SaveChanges();
                return RedirectToAction(nameof(Index)); 
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
        public IActionResult DeleteImage(int id)
        {
            var source = _context.DS_VDVs.Find(id);
            string wwwRootPath = _webHost.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/PlayerImg/", source.File_Anh);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                source.File_Anh = null;
                var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, new List<string> { "File_Anh" });
                if (result.Succeeded) _context.SaveChanges();
            }
            return RedirectToAction(nameof(Update), id);
        }
    }
}
