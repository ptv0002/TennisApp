using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Areas.NoRole.Models
{
    public class RegisterViewModel : DS_VDV
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
