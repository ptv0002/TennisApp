using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tennis_Web.Models;

namespace Tennis_Web.Areas.NoRole.Models
{
    public class ResultViewModel
    {
        public Tab ActiveTab { get; set; }
        public bool IsCurrent { get; set; }
        public int ID { get; set; }
        public string Tournament { get; set; }
        public string Level { get; set; }
    }
}
