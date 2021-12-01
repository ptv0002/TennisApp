using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.IO;

namespace Models
{
    public class JSON_Class
    {
        public static void JSON_Write<T> (string JS_File, T context)
        {
            File.WriteAllText(JS_File, JsonConvert.SerializeObject(context, Formatting.Indented));
        }
        public static HashSet<T> JSON_File<T>(string JS_File)
        {
            using StreamReader r = new StreamReader(JS_File);
            string json = r.ReadToEnd();
            HashSet<T> danhsach = JsonConvert.DeserializeObject<HashSet<T>>(json);
            return danhsach;
        }
    }
}
   


