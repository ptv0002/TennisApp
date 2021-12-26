using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class TournamentTabViewModel
    {
        public Tab ActiveTab { get; set; }
        public bool? IsCurrent { get; set; }
        public int? ID { get; set; }
        public string DetailedTitle { get; set; }
    }
    public enum Tab
    {
        Info,
        Parameter,
        Player,
        Division
    }
    public class PlayerTab
    {
        public int? Id { get; set; }
        public IEnumerable<DS_VDV> PlayerList { get; set; }
        public TournamentTabViewModel ViewModel { get; set; }
    }
}
