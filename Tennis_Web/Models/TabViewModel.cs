using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class TabViewModel
    {
        public Tab ActiveTab { get; set; }
        public bool IsCurrent { get; set; }
        public int? ID { get; set; }
        public string DetailedTitle { get; set; }
        public object CurrentModel { get; set; }
        public bool? Succeeded { get; set; }
    }
    public enum Tab
    {
        Info,
        Parameter,
        Player,
        Division,
        LevelList,
        Pair
    }
}
