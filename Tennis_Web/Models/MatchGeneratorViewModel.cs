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
        public List<NumPerTable> NumPerTable { get; set; }
    }
    public class NumPerTable
    {
        public char Table { get; set; }
        public int Num { get; set; }
    }
    public class MatchGeneratorErrorViewModel
    {
        public IEnumerable<DS_Cap> NoCodePairs { get; set; }
        public IEnumerable<DS_VDV> NoPairPlayers { get; set; }
    }
}
