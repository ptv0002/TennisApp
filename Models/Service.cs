using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Models
{
    public class Srv_ThongBao
    {
        private readonly string _dataFile = @"C:\TEMP\ThongBao.Txt";
        public string[] DS_ThongBaos { get; set; }
        public Srv_ThongBao()
        {
            DS_ThongBaos = File.ReadAllLines(_dataFile);
            ;
        }

        public string[] Get() => DS_ThongBaos;
    }

}
public class Srv_VDV
    {
        private readonly string _dataFile = @"C:\TEMP\DS_VDV.JS";
        //using StreamReader r = new StreamReader(_dataFile);
        //string json = r.ReadToEnd();
        //var ds_cap = JsonConvert.DeserializeObject<DS_Cap>(json);
        //private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<Book>));
        public HashSet<DS_VDV> DS_VDVs { get; set; }
        public Srv_VDV()
        {
            DS_VDVs = JSON_Class.JSON_File<DS_VDV>(_dataFile);
        }

        public DS_VDV[] Get() => DS_VDVs.ToArray();

        public DS_VDV Get(int id) => DS_VDVs.FirstOrDefault(b => b.Id == id);

        public bool Add(DS_VDV vdv) => DS_VDVs.Add(vdv);

        public DS_VDV Create()
        {
            var max = DS_VDVs.Max(b => b.Id);
            var b = new DS_VDV()
            {
                Id = max + 1,
            };
            return b;
        }

        public bool Update(DS_VDV vdv)
        {
            var b = Get(vdv.Id);
            return b != null && DS_VDVs.Remove(b) && DS_VDVs.Add(vdv);
        }

        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && DS_VDVs.Remove(b);
        }
        public void SaveChanges()
        {
            JSON_Class.JSON_Write<HashSet <DS_VDV>>(_dataFile, DS_VDVs);
        }
        public string GetDataPath(string file) => $"Data\\{file}";

        /*
        public void Upload(DS_VDV book, IFormFile file)
        {
            if (file != null)
            {
                var path = GetDataPath(file.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                file.CopyTo(stream);
                book.DataFile = file.FileName;
            }
        }
        */
        /*
        public (Stream, string) Download(Book b)
        {
            var memory = new MemoryStream();
            using var stream = new FileStream(GetDataPath(b.DataFile), FileMode.Open);
            stream.CopyTo(memory);
            memory.Position = 0;
            var type = Path.GetExtension(b.DataFile) switch
            {
                "pdf" => "application/pdf",
                "docx" => "application/vnd.ms-word",
                "doc" => "application/vnd.ms-word",
                "txt" => "text/plain",
                _ => "application/pdf"
            };
            return (memory, type);
        }
        */
        /*public DS_VDV[] Get(string search)
        {
            var s = search.ToLower();
            return DS_VDVs.Where(b =>
                b.Name.ToLower().Contains(s) ||
                b.Authors.ToLower().Contains(s) ||
                b.Publisher.ToLower().Contains(s) ||
                b.Year.ToString() == s
            ).ToArray();
            //b.Description.Contains(s) ||
        }
        */
        /*public (Book[] books, int pages, int page) Paging(int page)
        {
            int size = 5;
            int pages = (int)Math.Ceiling((double)Books.Count / size);
            var books = Books.Skip((page - 1) * size).Take(size).ToArray();
            return (books, pages, page);
        }*/
        /*
        public (Book[] books, int pages, int page) Paging(int page, string orderBy = "Name", bool dsc = false)
        {
            int size = 5;
            int pages = (int)Math.Ceiling((double)Books.Count / size);
            var books = Books.Skip((page - 1) * size).Take(size).AsQueryable().OrderBy($"{orderBy} {(dsc ? "descending" : "")}").ToArray();
            return (books, pages, page);
        }*/
    }
    public class Srv_cap
    {
        private readonly string _dataFile = @"C:\TEMP\DS_Cap.JS";
        public HashSet<Models.DS_Cap> DS_Caps { get; set; }
        public Srv_cap()
        {
            DS_Caps = JSON_Class.JSON_File<DS_Cap>(_dataFile);
        }

        public DS_Cap[] Get() => DS_Caps.ToArray();

        public DS_Cap Get(int id) => DS_Caps.FirstOrDefault(b => b.Id == id);

        public bool Add(DS_Cap capvdv) => DS_Caps.Add(capvdv);

        public DS_Cap Create()
        {
            var max = DS_Caps.Max(b => b.Id);
            var b = new DS_Cap()
            {
                Id = max + 1,
            };
            return b;
        }

        public bool Update(DS_Cap capvdv)
        {
            var b = Get(capvdv.Id);
            return b != null && DS_Caps.Remove(b) && DS_Caps.Add(capvdv);
        }

        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && DS_Caps.Remove(b);
        }
        public void SaveChanges()
        {
            JSON_Class.JSON_Write<HashSet<DS_Cap>>(_dataFile, DS_Caps);
        }
    }
