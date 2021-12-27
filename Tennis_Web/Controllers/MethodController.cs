using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Controllers
{
    public class MethodController : Controller
    {
        private readonly TennisContext _context;
        private readonly IWebHostEnvironment _environment;
        public MethodController(TennisContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public class ConfirmViewModal
        {
            public string BtnMsg { get; set; }
            public string Message { get; set; }
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public object RouteValues { get; set; }
        }
        public int GetColumn(string columnName, ExcelWorksheet sheet)
        {
            var idx = sheet.Cells["1:1"].First(c => c.Value.ToString() == columnName).Start.Column;
            return idx;
        }
        public ExcelWorksheet GetWorkSheet(string sheetName)
        {
            var package = GetExcel();
            return package.Workbook.Worksheets[sheetName];
        }
        public ExcelWorksheet GetWorkSheet(int index)
        {
            var package = GetExcel();
            return package.Workbook.Worksheets[index];
        }
        public List<DS_Trinh> GetLevelList()
        {
            var levels = new List<DS_Trinh>();
            var temp = new MethodController(_context, _environment);
            var levSheet = temp.GetWorkSheet("DS_Trinh");
            int rowCount = levSheet.Dimension.End.Row;
            for (int row = 2; row < rowCount + 1; row++)
            {
                DS_Trinh item = new()
                {
                    Id = Convert.ToInt32(levSheet.Cells[row, temp.GetColumn("Id", levSheet)].Text),
                    Trinh = Convert.ToInt32(levSheet.Cells[row, temp.GetColumn("Trinh", levSheet)].Text)
                };
                levels.Add(item);
            }
            return levels;
        }
        public ExcelPackage GetExcel()
        {
            // Index | Sheet
            //   0   | DS_Bang
            //   1   | DS_Cap
            //   2   | DS_Diem
            //   3   | DS_Giai
            //   4   | DS_Tran
            //   5   | DS_Trinh
            //   6   | DS_VDV
            //   7   | DS_Vong
            var path = Path.Combine(_environment.WebRootPath, "Files/") + "Giai_Dau.xlsx";
            FileInfo file = new(path);
            return new ExcelPackage(file);
        }
        public void ImportFromExcel(ExcelPackage excel)
        {

        }
        public IActionResult ConfirmModal(ConfirmViewModal model)
        {
            return PartialView(model);
        }
        [HttpPost]
        public IActionResult ConfirmModal(ConfirmViewModal model, string any = "")
        {
            return RedirectToAction(model.ActionName, model.ControllerName, model.RouteValues);
        }
    }
}
