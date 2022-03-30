using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Areas.NoRole.Models
{
    public class PlayerHistoryViewModel
    {
        public DS_VDV VDV { get; set; }
        public List<Extended_VDVDiem> DS_VDVDiem { get; set; }
        public List<DS_Tran> DS_Tran { get; set; }
    }
    public class Extended_VDVDiem : DS_VDVDiem
    {
        public int Diem_Cu { get; set; }
        public int Diem_Moi { get; set; }
    }
}
