using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class FileMethod
    {
        private readonly DbContext _context;
        private readonly IWebHostEnvironment _webHost;
        public FileMethod(DbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public void DeleteImage(string id)
        {
            var source = _context.Set<DS_VDV>().Find(Convert.ToInt32(id));
            string wwwRootPath = _webHost.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/Files/PlayerImg/", source.File_Anh);
            if (File.Exists(path))
            {
                File.Delete(path);
                source.File_Anh = null;
                var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(id, source, new List<string> { "File_Anh" });
                if (result.Succeeded) _context.SaveChanges();
            }
        }
        public void SaveImage(DS_VDV source)
        {
            string extension = Path.GetExtension(source.Picture.FileName);
            // Delete image if already exists
            string wwwRootPath = _webHost.WebRootPath + "/Files/PlayerImg/";
            if (source.File_Anh != null)
            {
                string existPath = Path.Combine(wwwRootPath, source.File_Anh);
                if (File.Exists(existPath)) System.IO.File.Delete(existPath);
            }

            // Save image to wwwroot/PlayerImg
            string fileName = Path.GetFileNameWithoutExtension(source.Picture.FileName);
            source.File_Anh = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath, fileName);
            using var fileStream = File.Create(path);
            source.Picture.CopyTo(fileStream);
            fileStream.Dispose();
        }
        public void SaveAnnouncement(Thong_Bao source)
        {
            string extension = Path.GetExtension(source.File.FileName);
            // Delete image if already exists
            string wwwRootPath = _webHost.WebRootPath + "/Files/Announcement/";
            if (source.File_Path != null)
            {
                string existPath = Path.Combine(wwwRootPath, source.File_Path);
                if (File.Exists(existPath)) File.Delete(existPath);
            }

            // Save image to wwwroot/Announcement
            string fileName = Path.GetFileNameWithoutExtension(source.File.FileName);
            source.File_Path = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath, fileName);
            using var fileStream = File.Create(path);
            source.File.CopyTo(fileStream);
            fileStream.Dispose();
        }
    }
}
