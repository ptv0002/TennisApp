using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

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
        /// Hàm cho tên đầy đủ của file ảnh, nếu có - Chưa có phần mở rộng
        /// </summary>
        /// <param name="id"></param>
        public string NameImage(int id)
        {
            string FileAnh=id.ToString();
            return Path.Combine(_webHost.WebRootPath + "/uploads/PlayerImg/", FileAnh);
        }
        /// <summary>
        /// Hàm kiểm tra và cho tên file ảnh có phần mở rộng (không lấy thư mục đầy đủ chỉ lấy tên)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string NameImageTest(int id)
        {
            string FileAnh = id.ToString();
            string path = Path.Combine(_webHost.WebRootPath + "/uploads/PlayerImg/", FileAnh);
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
            string path = Path.Combine(wwwRootPath + "/uploads/PlayerImg/", source.File_Anh);
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
            string path = Path.Combine(_webHost.WebRootPath + "/uploads/PlayerImg/", fileOld);
            string fileNew = NameImage(source.Id) + extension;
            if (File.Exists(path)) System.IO.File.Delete(path);    // Xóa file cũ nếu có
            //            if (fileOld != null) System.IO.File.Delete(fileOld);    // Xóa file cũ nếu có
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
            string wwwRootPath = _webHost.WebRootPath + "/uploads/PlayerImg/";
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
            string wwwRootPath = _webHost.WebRootPath + "/uploads/Announcement/";
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
        public bool TestLicence(string domain, string name)
        {
            IPAddress[] ip = Dns.GetHostAddresses(domain);
            string file_Test = HexPlus(HexFromString(ip[0].ToString() + name), HexFromString("Cong ty TNHH Tin Hung - 56 Quang Trung - Nha Trang"), '+');
            file_Test = _webHost.WebRootPath + "/uploads/PlayerImg/"+ file_Test+".png";
            return File.Exists(file_Test);
        }
        /// <summary>
        /// Cộng 2 số Hex - Nhưng cộng/trừ từ trái sang phải (không theo cách thông thường) - Chỉ để mã hóa thôi
        /// </summary>
        /// <param name="_Num1"></param>
        /// <param name="_Num2"></param>
        /// <param name="_Dau"></param>
        /// <returns></returns>
        static string HexPlus(string _Num1, string _Num2, char _Dau)
        {
            string result = "";
            if (_Num1.Length % 2 == 1) { _Num1 = "0" + _Num1; }
            if (_Num2.Length % 2 == 1) { _Num2 = "0" + _Num2; }

            int min = Math.Min(_Num1.Length, _Num2.Length);
            int nho  = 0 ;
            byte[] _C1, _C2;
            byte temp;
            for (int i=0 ; i*2 < min ; i++)
            {
                _C1 = Convert.FromHexString(_Num1.Substring(i * 2, 2));
                _C2 = Convert.FromHexString(_Num2.Substring(i * 2, 2));
                if (_Dau == '+')
                {
                    temp = (byte)(_C1[0] + _C2[0] + nho);
                    if ((_C1[0] + _C2[0] + nho) >= 256)
                    {
                        nho = 1;
                    }
                    else
                    {
                        nho = 0;
                    }
                }
                else
                {
                    temp = (byte)(_C1[0] - _C2[0] + nho);
                    if ((_C1[0] - _C2[0] + nho) < 0)
                    {
                        nho = -1;
                    }
                    else
                    {
                        nho = 0;
                    }
                }
                result += temp.ToString("X");
            }
            return result;
        }
        static string HexFromString (string _Chuoi)
        {
            string result = "";
            byte _Code;
            char _CCode;
            for (int i=0; i<_Chuoi.Length;i++) 
            {
                _Code = (byte)_Chuoi[i];
                _Code %= 16;
                if (_Code <= 9)
                {
                    _CCode = (char) (_Code+48);
                }
                else
                {
                    _CCode = (char)(_Code + 55);
                }
                result += _CCode;
            }
            return result;
        }
    }
}
