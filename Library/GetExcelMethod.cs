using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class GetExcelMethod
    {
        public GetExcelMethod()
        {
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
            //FileInfo file = new(/*path*/);
            return new ExcelPackage(/*file*/);
        }
    }
}
