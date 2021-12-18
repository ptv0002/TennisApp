using AspNetCoreHero.ToastNotification.Abstractions;
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
            return View();
        }
        public IActionResult ExportExcel()
        {
            var model = _context.DS_Trinhs.Include(m => m.DS_Giai).OrderByDescending(m => m.DS_Giai.Ngay).ThenByDescending(m => m.Trinh).ToList();
            return View(model);
        }
        public IActionResult Import(bool isPartial, ImportExcelViewModel model)
        {
            // Delete entire DB data and add new data from Excel
            if (isPartial == false) 
            { 
                
            }
            // Add on to DB from Excel
            else 
            {

            }
            return View();
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
                //_context.Model.GetEntityTypes().ElementAt(1).GetTableName()
                //var types = Assembly.LoadFrom("D:/Data/Visual Studio Projects/TennisApp/Models/bin/Debug/net5.0/Models.dll").GetTypes();

                foreach (var t in types) 
                {
                    //bool a1 = t.Name == "AppUser";
                    //bool a2 = t.Name == "Thong_Bao";
                    //bool a3 = t.Name == "Khu_Vuc";
                    // Create sheets for every class, except for those listed above
                    if ((t.DisplayName().StartsWith("DS_")))
                    {
                        // Add sheet for every class
                        var sheet = package.Workbook.Worksheets.Add(t.DisplayName());
                        int i = 1;
                        foreach (var f in t.GetProperties()) 
                        {
                            // Print column headers, except for some conditions below
                            //if (!(f.Name == "Id" || f.PropertyType.Name == "ICollection`1"))
                            //if (!(f.Name == "Id" || f.PropertyInfo.Name == "ICollection`1"))
                            //{
                                sheet.Cells[1, i].Value = f.Name;
                                i++;
                            //}    
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
            // Eventually switching 
            //string path = "/Doc/Giải Đấu.xlsx";
            //return File(path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Giải Đấu.xlsx");
        }
    }
}
