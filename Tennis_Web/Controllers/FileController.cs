using AspNetCoreHero.ToastNotification.Abstractions;
using DataAccess;
using Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly INotyfService _notyf;
        public FileController(TennisContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IActionResult ImportExcel()
        {
            return View(GetEntityLists());
        }
        [HttpPost]
        public IActionResult ImportExcel(IFormFile file, List<EntityListViewModel> list)
        {
            if(list.All(m => m.IsSelected == false))
            {
                ModelState.AddModelError(string.Empty, "Chọn ít nhất 1 danh sách để nhập dữ liệu!");
                return View(list);
            }
            List<EntityListViewModel> ds_table = (List<EntityListViewModel>)list.Select(s => s.IsSelected);

            for (int i = 0; i < ds_table.Count; i++)
            {
                var type = Type.GetType(ds_table[i].EntityName);
                var instance = Activator.CreateInstance(type);
                var excelToList = new ExcelMethod<DS_Giai>().ExcelToList(file, ds_table[i].EntityName); 

                if (!excelToList.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, excelToList.Message);
                    return View(list);
                }
                _context.AddRange(excelToList.List);
                _context.SaveChanges();
            }
            return View(list);
        }
        public List<EntityListViewModel> GetEntityLists()
        {
            var list = new List<EntityListViewModel>();
            var types = _context.Model.GetEntityTypes();

            foreach (var t in types)
            {
                if (t.DisplayName().StartsWith("DS_")) list.Add(new EntityListViewModel() { EntityName = t.DisplayName() });
            }
            return list;
        }
        public IActionResult ExportExcel()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult Export(int id)
        {

            return View();
        }
        // Download empty Excel format
        public FileResult DownloadExcel(int id)
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
