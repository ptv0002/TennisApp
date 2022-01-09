using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Library
{
    public class ExcelMethod<T> where T : new()
    {
        public ResultModel<T> ExcelToList(IFormFile file, string sheetName)
        {
            var model = new ResultModel<T>();
            // Check if file is empty
            if (file == null || file.Length <= 0)
            {
                model.Succeeded = false;
                model.Message = "File is empty!";
                return model;
            }
            // Get file extension from file name and check if file is in the right format
            var extension = Path.GetExtension(file.FileName);
            if (!(extension == ".xlsx" || extension == ".xls"))
            {
                model.Succeeded = false;
                model.Message = "Wrong file format!";
                return model;
            }
            var ms = new MemoryStream();
            file.CopyTo(ms);
            var excel = new ExcelPackage(ms);
            return ToList(excel, sheetName);
        }
        private ResultModel<T> ExcelToList(string path, string sheetName)
        {
            var model = new ResultModel<T>();
            FileInfo file = new(path);
            if (!(file.Extension == ".xlsx" || file.Extension == ".xls"))
            {
                model.Succeeded = false;
                model.Message = "Wrong file format!";
                return model;
            }
            var excel = new ExcelPackage(path);
            return ToList(excel, sheetName);
        }
        private ResultModel<T> ToList(ExcelPackage excel, string sheetName)
        {
            var model = new ResultModel<T>();
            // Check sheetName có trong file Excel ko
            if (excel.Workbook.Worksheets[sheetName] == null)
            {
                model.Succeeded = false;
                model.Message = "Sheet name doesn't exist in Excel File!";
                return model;
            };
            // Check if sheetName is valid
            if (!(typeof(T).Name == sheetName))
            {
                model.Succeeded = false;
                model.Message = "Sheet name doesn't match provided Type!";
                return model;
            }
            var worksheet = excel.Workbook.Worksheets[sheetName];
            var listrows = new List<T>();
            var listcols = new Dictionary<int, PropertyInfo>();
            foreach (var prop in typeof(T).GetProperties())  // Lấy tất cả các thuộc tính của T (Các cột của Table/Thuộc tính của Models.T)
            {
                int col = GetColumn(prop.Name, worksheet);  // col = 0 --> không có cột trên file Excel --> bỏ qua
                if (col != 0) listcols.Add(col, prop);
            }
            for (int row = 2; row < worksheet.Dimension.End.Row + 1; row++)
            {
                T type = new();
                foreach (var ocol in listcols)  // Lấy tất cả các thuộc tính của T (Các cột của Table/Thuộc tính của Models.T)
                {
                    var cellValue = worksheet.Cells[row, ocol.Key].Value;
                    var propType = ocol.Value.PropertyType;
                    if (Nullable.GetUnderlyingType(ocol.Value.PropertyType) != null)
                    {
                        propType = Nullable.GetUnderlyingType(ocol.Value.PropertyType);
                    }
                    else if ((cellValue == null) && (ocol.Value.PropertyType.Name.ToString().ToUpper() != "STRING"))
                    {
                        model.Succeeded = false;
                        model.Message = "'" + ocol.Value.Name + "' column cannot be empty!";
                        return model;
                    }
                    ocol.Value.SetValue(type, Convert.ChangeType(cellValue, propType));
                }
                listrows.Add(type);
            }
            model.Succeeded = true;
            model.List = listrows;
            return model;
        }
        private int GetColumn(string columnName, ExcelWorksheet sheet)
        {
            var testcolumn = sheet.Cells["1:1"].FirstOrDefault(c => c.Value.ToString().ToUpper() == columnName.ToUpper());
            var idx = 0;
            if (testcolumn != null) idx = testcolumn.Start.Column;
            return idx;
        }
    }
}
