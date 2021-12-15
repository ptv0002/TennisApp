using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tennis_Web.Controllers
{
    public class FileController : Controller
    {
        private readonly TennisContext _context;
        public FileController(TennisContext context)
        {
            _context = context;
        }
        public IActionResult ImportExcel()
        {
            return View();
        }
        public IActionResult ExportExcel()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult Import(bool id)
        {

            return View();
        }
        public IActionResult Export(int id)
        {

            return View();
        }
        public IActionResult EmptyExcel(int id)
        {
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                // "D:/Data/Visual Studio Projects/TennisApp/Models/bin/Debug/net5.0/Models.dll"
                var types = Assembly.LoadFrom("../../TennisApp/Models/bin/Debug/net5.0/Models.dll").GetTypes();
                foreach (var t in types) 
                {
                    bool a1 = t.Name == "AppUser";
                    bool a2 = t.Name == "DS_ThongBao";
                    bool a3 = t.Name == "Khu_Vuc";
                    // Create sheets for every class, except for those listed above
                    if (!(a1 || a2 || a3))
                    {
                        // Add sheet for every class
                        var sheet = package.Workbook.Worksheets.Add(t.Name);
                        int i = 1;
                        foreach (var f in t.GetProperties()) 
                        {
                            // Print column headers, except for some conditions below
                            if (!(f.Name == "Id" || f.PropertyType.Name == "ICollection`1"))
                            {
                                sheet.Cells[1, i].Value = f.Name;
                                i++;
                            }    
                        }
                        // Format Column Headers and Column size
                        sheet.Columns.AutoFit();
                        sheet.Row(1).Style.Font.Bold = true;
                    }
                }
                package.Save();
            }
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Giải Đấu.xlsx");
        }
    }
}
