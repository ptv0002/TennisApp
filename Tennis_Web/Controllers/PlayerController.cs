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
            var model = _context.DS_VDVs.OrderByDescending(m => m.Diem).ToList();
            return View(model);
        }
        public IActionResult Update(int id)
        {
            var destination = _context.DS_VDVs.Find(id);
            if (destination != null && destination.File_Anh != null) _notyf.Warning("Upload ảnh mới sẽ xóa ảnh cũ!", 100);
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
                    if (source.Picture.Length <= 250000)
                    {
                        // Delete image if already exists
                        var temp = _context.DS_VDVs.Find(id);
                        string wwwRootPath = _webHost.WebRootPath + "/Files/PlayerImg/";
                        string existPath = Path.Combine(wwwRootPath, source.File_Anh);
                        if (System.IO.File.Exists(existPath)) System.IO.File.Delete(existPath);

                        // Save image to wwwroot/PlayerImg
                        string fileName = Path.GetFileNameWithoutExtension(source.Picture.FileName);
                        source.File_Anh = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath, fileName);
                        using var fileStream = System.IO.File.Create(path);
                        await source.Picture.CopyToAsync(fileStream);
                        await fileStream.DisposeAsync();
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
            var columnsToSave = new List<string> { "Ho", "Ten", "Ten_Tat", "Gioi_Tinh", "CLB", "Khach_Moi", "File_Anh", "Tel", "Email", "Status", "Cong_Ty", "Chuc_Vu" };
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, columnsToSave);
            if (result.Succeeded) 
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
        public IActionResult DeleteImage(string id)
        {
            var source = _context.DS_VDVs.Find(Convert.ToInt32(id));
            string wwwRootPath = _webHost.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/Files/PlayerImg/", source.File_Anh);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                source.File_Anh = null;
                var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, new List<string> { "File_Anh" });
                if (result.Succeeded) _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Update), id);
        }
    }
}
