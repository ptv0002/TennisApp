using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class DatabaseMethod<T> where T : class, new()
    {
        private readonly DbContext _context;
        public DatabaseMethod(DbContext context)
        {
            _context = context;
        }
        public T GetOjectFromDB(object id)
        {
            var destination = new T();
            if (id is not null and not 0)
            {
                // Get object from DB with given id
                var source = _context.Set<T>().Find(id);
                // Get all info from DB object and save to Destination object
                foreach (var prop in typeof(T).GetProperties())
                {
                    prop.SetValue(destination, prop.GetValue(source));
                }
            }
            return destination;
        }
        /// <summary>
        /// Lưu dữ liệu kiểu T theo danh sách cột vào 1 đối tượng trong context
        /// </summary>
        /// <param name="id"></param> : Đối tượng cần lưu, hoặc nhập mới
        /// <param name="source"></param> : Dữ liệu kiểu T cần lưu vào đối tượng
        /// <param name="columnsToSave"></param> : Danh sách các cột - ĐÃ HỢP LỆ (Chữ in)
        /// <returns></returns>
        public ResultModel<T> SaveObjectToDB(object id, T source, List<string> columnsToSave)
        {
            // --------------------- Possible error ---------------------
            // 1. Not null field doesn't get filled when ADDing new object
            // 2. Update null value to a not null field (extra column in columnsToSave)
            // ----------------------------------------------------------
            var model = new ResultModel<T>();
            columnsToSave = TestListCol(columnsToSave);
            var destination = _context.Set<T>().Find(id);
            if (destination == null) destination = new T();
            PropertyInfo prop;
            foreach (var col in columnsToSave)
            {
                    prop = typeof(T).GetProperties().First(m => m.Name.ToUpper() == col);
                    prop.SetValue(destination, prop.GetValue(source));
            }
            // If id is null, Update object to DB, if not null, Add object to DB
            if (id is not null and not 0)
            {
                _context.Update(destination);
            }
            else _context.Add(destination);
            
            model.Succeeded = true;
            model.Message = "Save to database successfully!";
            return model;
        }
        /// <summary>
        /// Lưu dữ liệu kiểu T theo danh sách cột vào 1 đối tượng trong context
        /// </summary>
        /// <param name="listObject"></param> : Đối tượng cần lưu, hoặc nhập mới
        /// <param name="columnsToSave"></param> : Danh sách các cột - ĐÃ HỢP LỆ (Chữ in)
        /// <returns></returns>
        public ResultModel<T> SaveListObjectToDB(List<T> listObject, List<string> columnsToSave)
        {
            var model = new ResultModel<T>();
            columnsToSave = TestListCol(columnsToSave);
            if (columnsToSave.Count == 0)
            {
                model.Succeeded = false;
                model.Message = "Not data !";
                return model;
            }
            foreach (var obj in listObject)
            {
                model = SaveObjectToDB(null, obj, columnsToSave);
                if (!model.Succeeded)
                {
                    model.Succeeded = false;
                    model.Message = "Update Error !";
                    return model;
                }
            }
            if (!model.Succeeded)
            {
                model.Succeeded = true;
                model.Message = "Save to database successfully!";
                return model;
            }
            _context.SaveChanges();
            return model;
        }
        /// <summary>
        /// Lấy danh sách các cột hợp lệ, nếu danh sách bằng rỗng --> không có cột nào hợp lệ
        /// </summary>
        /// <param name="source"></param>
        /// <param name="columnsToSave"></param>
        /// <returns></returns>
        private List<string> TestListCol (List<string> columnsToSave)
        {
            List<string> listCol = new();
            columnsToSave = columnsToSave.Select(m => m.ToUpper()).ToList();
            foreach (var prop in typeof(T).GetProperties())  // Lấy tất cả các thuộc tính của T (Các cột của Table/Thuộc tính của Models.T)
            {
                if (columnsToSave.Contains(prop.Name.ToUpper()) )
                {
                    listCol.Add(prop.Name.ToUpper());
                }
            }
            return listCol;
        }
    }
}
