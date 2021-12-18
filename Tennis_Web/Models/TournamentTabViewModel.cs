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
        public int? TrinhID { get; set; }
    }
    public enum Tab
    {
        Parameter,
        Player,
        Division
    }
}
