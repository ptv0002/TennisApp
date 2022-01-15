using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ResultModel
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<object> List { get; set; }
        public Dictionary<int, PropertyInfo> ListCol { get; set; }
    }
}
