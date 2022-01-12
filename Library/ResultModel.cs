using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ResultModel<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<T> List { get; set; }
        public Dictionary<int, PropertyInfo> ListCol { get; set; }
    }
}
