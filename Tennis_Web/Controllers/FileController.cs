using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Controllers
{
    public class FileController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _webHost;
        private readonly INotyfService _notyf;
        public FileController(TennisContext context, IWebHostEnvironment webHost, INotyfService notyf)
        {
            _context = context;
            _webHost = webHost;
            _notyf = notyf;
        }
        public IActionResult AnnouncementIndex()
        {
            var model = _context.Thong_Baos.OrderByDescending(m => m.Ngay).ToList();
            return View(model);
        }
        public IActionResult UpdateAnnouncement(int id)
        {
            var destination = _context.Thong_Baos.Find(id);
            if (destination != null && destination.File_Path != null) _notyf.Warning("Upload file thông báo mới sẽ xóa file cũ!", 100);
            ViewBag.GiaiList = new SelectList(_context.DS_Giais.OrderByDescending(m => m.Ngay), "Id", "Ten");
            return View(destination);
        }
        [HttpPost]
        public IActionResult UpdateAnnouncement(int id, Thong_Bao source)
        {
            var column = "";
            ViewBag.GiaiList = new SelectList(_context.DS_Giais.OrderByDescending(m => m.Ngay), "Id", "Ten");
            switch (source.File == null)
            {
                case true:
                    if (source.File_Text == null && source.File_Path == null)
                    {
                        ModelState.AddModelError(string.Empty, "Soạn thảo hoặc upload file thông báo!");
                        return View(source);
                    }
                    else break;
                case false:
                    // Handle file attachment
                    string extension = Path.GetExtension(source.File.FileName);
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".pdf")
                    {
                        //// Delete image if already exists
                        //string wwwRootPath = _webHost.WebRootPath + "/Files/Announcement/";
                        //if (source.File_Path != null)
                        //{
                        //    string existPath = Path.Combine(wwwRootPath, source.File_Path);
                        //    if (System.IO.File.Exists(existPath)) System.IO.File.Delete(existPath);
                        //}

                        //// Save image to wwwroot/PlayerImg
                        //string fileName = Path.GetFileNameWithoutExtension(source.File.FileName);
                        //source.File_Path = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        //string path = Path.Combine(wwwRootPath, fileName);
                        //using var fileStream = System.IO.File.Create(path);
                        //source.File.CopyTo(fileStream);
                        //fileStream.Dispose();
                        new FileMethod(_context, _webHost).SaveAnnouncement(source);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Dạng file " + extension + " không được hỗ trợ!");
                        return View(source);
                    }
                    column = "File_Path";
                    break;
            }
            // Handle saving object
            var columnsToSave = new List<string> { "Ten", "Ngay", "Hien_Thi", "Tin_Nong" ,"ID_Giai", "File_Text", column };
            var result = new DatabaseMethod<Thong_Bao>(_context).SaveObjectToDB(id, source, columnsToSave);
            if (result.Succeeded)
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(AnnouncementIndex));
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View(source);
        }
        public IActionResult DeleteAnnouncement(string id)
        {
            var source = _context.Thong_Baos.Find(Convert.ToInt32(id));
            if (source.File_Path != null)
            {
                string wwwRootPath = _webHost.WebRootPath;
                string path = Path.Combine(wwwRootPath + "/Files/Announcement/", source.File_Path);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
           
            _context.Remove(source);
            _context.SaveChanges();
            return RedirectToAction(nameof(AnnouncementIndex));
        }
        public IActionResult DeleteFile(string id)
        {
            var source = _context.Thong_Baos.Find(Convert.ToInt32(id));
            string wwwRootPath = _webHost.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/Files/Announcement/", source.File_Path);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                source.File_Path = null;
                var result = new DatabaseMethod<Thong_Bao>(_context).SaveObjectToDB(id, source, new List<string> { "File_Path" });
                if (result.Succeeded) _context.SaveChanges();
            }
            return RedirectToAction(nameof(UpdateAnnouncement), Convert.ToInt32(id));
        }
        public IActionResult ImportExcel()
        {
            bool? success = (bool?)TempData["SuccessfulImport"];
            if (success == true) _notyf.Success("Đã cập nhật các Sheet từ Excel thành công!"); 
            return View(GetEntityLists());
        }
        [HttpPost]
        public IActionResult ImportExcel(IFormFile file, List<EntityListViewModel> list)
        {
            // Kiểm tra chọn file
            if (file == null)
            {
                ModelState.AddModelError(string.Empty, "Chưa chọn file Excel để cập nhật!");
                return View(list);
            }

            if (list.All(m => m.IsSelected == false))
            {
                ModelState.AddModelError(string.Empty, "Chọn ít nhất 1 danh sách để nhập dữ liệu!");
                return View(list);
            }
            var ds_table = (from a in list where a.IsSelected select a).ToList ();
            bool lerror = false;
            for (int i = 0; i < ds_table.Count; i++)
            {
                switch (ds_table[i].EntityName)
                {
                    case "DS_Giai":
                        var a1 = new ExcelMethod<DS_Giai>().ExcelToList(file,"DS_Giai");
                        if (a1.Succeeded) { _context.AddRange(a1.List);} else { ModelState.AddModelError(string.Empty, a1.Message); lerror = true; }
                        break;
                    case "DS_Trinh":
                        var a2 = new ExcelMethod<DS_Trinh>().ExcelToList(file, "DS_Trinh");
                        if (a2.Succeeded) { _context.AddRange(a2.List); } else { ModelState.AddModelError(string.Empty, a2.Message); lerror = true; }
                        break; 
                    case "DS_VDV":
                        var a3 = new ExcelMethod<DS_VDV>().ExcelToList(file, "DS_VDV");
                        if (a3.Succeeded) { _context.AddRange(a3.List); } else { ModelState.AddModelError(string.Empty, a3.Message); lerror = true; }
                        break;
                    case "DS_Cap":
                        var a4 = new ExcelMethod<DS_Cap>().ExcelToList(file, "DS_Cap");
                        if (a4.Succeeded) { _context.AddRange(a4.List); } else { ModelState.AddModelError(string.Empty, a4.Message); lerror = true; }
                        break;
                    case "DS_VDVDiem":
                        var a5 = new ExcelMethod<DS_VDVDiem>().ExcelToList(file, "DS_VDVDiem");
                        if (a5.Succeeded) { _context.AddRange(a5.List); } else { ModelState.AddModelError(string.Empty, a5.Message); lerror = true; }
                        break;
                    default:
                        ModelState.AddModelError(string.Empty, "Không được cập nhật từ Excel file này!");
                        lerror = true;
                        return View(list);
                }
            }
            if (! lerror) 
            { 
                //_context.SaveChangesAsync();
                _context.SaveChanges();
                TempData["SuccessfulImport"] = true;
                return RedirectToAction(nameof(ImportExcel));
            }
            return View(list);
        }
        public List<EntityListViewModel> GetEntityLists()
        {
            var list = new List<EntityListViewModel>
            {
                new EntityListViewModel() { EntityName = "DS_Giai" },
                new EntityListViewModel() { EntityName = "DS_VDV" },

                new EntityListViewModel() { EntityName = "DS_Trinh" },
                new EntityListViewModel() { EntityName = "DS_Cap" },
                new EntityListViewModel() { EntityName = "DS_VDVDiem" }
            };

            return list;
        }
        public IActionResult ExportExcel()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenBy(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult Export(int id)
        {

            return View();
        }
        // Download empty Excel format
        public FileResult DownloadExcel()
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var types = _context.Model.GetEntityTypes();

                foreach (var t in types) 
                {
                    if (t.DisplayName().StartsWith("DS_"))
                    {
                        // Add sheet for every class
                        var sheet = package.Workbook.Worksheets.Add(t.DisplayName());
                        int i = 1;
                        foreach (var f in t.GetProperties()) 
                        {
                            sheet.Cells[1, i].Value = f.Name;
                            i++;
                        }
                        // Format Column Headers and Column size
                        sheet.Columns.AutoFit();
                        sheet.Row(1).Style.Font.Bold = true;
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Giai_Dau.xlsx");
            // Eventually switching 
            //string path = "/Doc/Giải Đấu.xlsx";
        }
    }
}
