﻿using Library.FileInitializer;
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
        public int ID { get; set; }
        public bool Editable { get; set; }
        public string DetailedTitle { get; set; }
        public bool? Succeeded { get; set; }
        public string ErrorMsg { get; set; }
    }
    public class RoundTabViewModel
    {
        public int ID_Trinh { get; set; }
        public List<DS_Tran> DS_Tran { get; set; }
        public List<DS_Cap> DS_Cap { get; set; }
    }
    public class PointTabViewModel
    {
        public List<DS_Diem> DS_Diem { get; set; }
        public List<DS_Cap> DS_Cap { get; set; }
    }
    public enum Tab
    {
        Info,
        Parameter,
        Player,
        LevelList,
        Pair,
        Table,
        Special,
        Point
    }
}
