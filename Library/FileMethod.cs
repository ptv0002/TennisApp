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
        /// <summary>
        /// Hàm cho tên đầy đủ của file ảnh, nếu có
        /// </summary>
        /// <param name="id"></param>
        public string NameImage(int id)
        {
            string FileAnh=id.ToString();
            return Path.Combine(_webHost.WebRootPath + "\\Files\\PlayerImg\\", FileAnh);
        }
        public string NameImageTest(int id)
        {
            string FileAnh = id.ToString();
            string path = Path.Combine(_webHost.WebRootPath + "/Files/PlayerImg/", FileAnh);
            bool tontai = false;
            if (File.Exists(path + ".jpg")) { FileAnh += ".jpg"; tontai = true; }
            if (File.Exists(path + ".png")) { FileAnh += ".png"; tontai = true; }
            if (File.Exists(path + ".jpeg")) { FileAnh += ".jpeg"; tontai = true; }
            if (tontai)
            { return FileAnh; }
            else return null;
        }
        public void DeleteImage(string id)
        {
            var source = _context.Set<DS_VDV>().Find(Convert.ToInt32(id));
            string wwwRootPath = _webHost.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/Files/PlayerImg/", source.File_Anh);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            source.File_Anh = null;
            var result = new DatabaseMethod<DS_VDV>(_context).SaveObjectToDB(source.Id, source, new List<string> { "File_Anh" });
            if (result.Succeeded) _context.SaveChanges();
        }
        public void SaveImage(DS_VDV source)
        {
            string extension = Path.GetExtension(source.Picture.FileName);
            string fileOld = NameImageTest(source.Id);
            string fileNew = NameImage(source.Id) + extension;
            if (fileOld != null) System.IO.File.Delete(fileOld);    // Xóa file cũ nếu có
            // Chép file mới vào
            //string fileName = Path.GetFileNameWithoutExtension(source.Picture.FileName);
            using var fileStream = File.Create(fileNew);
            source.Picture.CopyTo(fileStream);
            fileStream.Dispose();
        }
        public void SaveImage1(DS_VDV source)
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
