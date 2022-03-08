using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class MatchGeneratorViewModel
    {
        public int ID_Trinh { get; set; }
        public int Trinh { get; set; }
        public int PlayOff1 { get; set; }
        public int PlayOff2 { get; set; }
        public List<ChosenPerTable> ChosenPerTable { get; set; }
    }
    public class ChosenPerTable
    {
        public char Table { get; set; }
        public int PairsNum { get; set; }
        public int Chosen { get; set; }
        public bool Playoff { get; set; }
    }
}
