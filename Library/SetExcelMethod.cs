using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

namespace Library
{
    public class SetExcelMethod<T> where T : new()
    {
        public class ResultModel
        {
            public bool Succeeded { get; set; }
            public string Message { get; set; }
            public List<T> List { get; set; }
        }
        public ResultModel ImportWorkSheet(IFormFile file, string sheetName, List<string> listOfColumns)
        {
            var model = new ResultModel();
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
            // Check if list of columns is valid
            if (!ValidColumnList(listOfColumns).Succeeded) return ValidColumnList(listOfColumns);
            // Check if 
            return model;
        }
        public ResultModel ImportWorkSheet(string path, string sheetName, List<string> listOfColumns)
        {
            var model = new ResultModel();
            FileInfo file = new(path);
            if (!(file.Extension == ".xlsx" || file.Extension == ".xls"))
            {
                model.Succeeded = false;
                model.Message = "Wrong file format!";
                return model;
            }
            var excel = new ExcelPackage(path);
            // Check if list of columns is valid
            if (!ValidColumnList(listOfColumns).Succeeded) return ValidColumnList(listOfColumns);
            if (!ToList(excel, sheetName).Succeeded) 
            {

            }
            return model;
        }
        public ResultModel ValidColumnList(List<string> listOfColumns)
        {
            var model = new ResultModel();
            foreach (var col in listOfColumns)
            {
                if(typeof(T).GetProperty(col) == null)
                {
                    model.Succeeded = false;
                    model.Message = "Column " + col + " is invalid!";
                }
                return model;
            }
            model.Succeeded = true;
            return model;
        }
        public ResultModel ToList(ExcelPackage excel, string sheetName)
        {
            // Check if sheetName is valid
            var model = new ResultModel();
            if (!(typeof(T).ToString() == sheetName))
            {
                model.Succeeded = false;
                model.Message = "Sheet name doesn't match provided Type!";
                return model;
            }
            var worksheet = excel.Workbook.Worksheets[sheetName];
            var list = new List<T>();
            for (int row = 2; row < worksheet.Dimension.End.Row; row++)
            {
                T type = new();
                foreach (var prop in typeof(T).GetProperties())
                {
                    Type propType = prop.PropertyType;
                    // Get property that needs to be set
                    var setProp = type.GetType().GetProperty(prop.Name);
                    var col = GetColumn(prop.Name, worksheet);
                    var cellValue = worksheet.Cells[row, Convert.ToInt32(col)].Text;

                    if (Nullable.GetUnderlyingType(propType) == null && cellValue == null) // Not nullable returns null
                    {
                        model.Succeeded = false;
                        model.Message = "Error occurred! " + prop.Name + " is a not nullable field.";
                        return model;
                    }
                    // Set value of a cell and try to convert to the target property of the Object 
                    setProp.SetValue(type, Convert.ChangeType(cellValue, propType));   
                }
                list.Add(type);
            }
            model.Succeeded = true;
            model.List = list;
            return model;
        }
        public ResultModel SaveToDatabase(DbContext context, List<T> list, List<string> listOfColumns)
        {
            var model = new ResultModel();
            var entity = context.Model.FindEntityType(typeof(T).ToString());
            if (entity == null)
            {
                model.Succeeded = false;
                model.Message = "Type can't be found in the provided context!";
                return model;
            }
            foreach(var col in listOfColumns)
            {
                
            }
            entity.GetProperties();    

            model.Succeeded = true;
            return model;
        }
        public int? GetColumn(string columnName, ExcelWorksheet sheet)
        {
            var idx = sheet.Cells["1:1"].FirstOrDefault(c => c.Value.ToString() == columnName).Start.Column;
            return idx;
        }
    }
}
