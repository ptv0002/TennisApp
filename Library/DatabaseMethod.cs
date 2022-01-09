using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
    public class DatabaseMethod<T> where T : class, new()
    {
        private readonly DbContext _context;
        public DatabaseMethod(DbContext context)
        {
            _context = context;
        }
        public async Task<T> GetOjectFromDBAsync(object id)
        {
            var destination = new T();
            if (id != null)
            {
                // Get object from DB with given id
                var source = await _context.Set<T>().FindAsync(id);
                // Get all info from DB object and save to Destination object
                foreach (var prop in typeof(T).GetProperties())
                {
                    prop.SetValue(destination, prop.GetValue(source));
                }
            }
            return destination;
        }
        public async Task<ResultModel<T>> SaveObjectToDBAsync(object id, T source, List<string> columnsToSave)
        {
            // --------------------- Possible error ---------------------
            // 1. Not null field doesn't get filled when ADDing new object
            // 2. Update null value to a not null field (extra column in columnsToSave
            // ----------------------------------------------------------
            var model = new ResultModel<T>();
            var destination = await _context.Set<T>().FindAsync(id);
            // Convert all column names to upper
            columnsToSave = columnsToSave.Select(m => m.ToUpper()).ToList();
            foreach(var prop in typeof(T).GetProperties())
            {
                // Check if property is in the list to save, if yes, save value and remove the column from the list
                if(columnsToSave.Contains(prop.Name.ToUpper()))
                {
                    prop.SetValue(destination, prop.GetValue(source));
                    columnsToSave.Remove(prop.Name.ToUpper());
                }
            }
            // If columnsToSave still has any element, return error message
            if (columnsToSave.Count == 1)
            {
                model.Succeeded = false;
                model.Message = columnsToSave.First() + " is not a valid column!";
                return model;
            }
            else if (columnsToSave.Count > 2)
            {
                model.Succeeded = false;
                foreach (var col in columnsToSave)
                {
                    if (col == columnsToSave.Last()) model.Message = model.Message + "and " + col;
                    else model.Message = model.Message + col + ", ";
                }
                model.Message += " are not valid columns!";
                return model;
            }
            // If id is null, Update object to DB, if not null, Add object to DB
            if (id != null)
            {
                _context.Update(destination);
            }
            else _context.Add(destination);
            await _context.SaveChangesAsync();
            model.Succeeded = true;
            model.Message = "Save to database successfully!";
            return model;
        }
    }
}
