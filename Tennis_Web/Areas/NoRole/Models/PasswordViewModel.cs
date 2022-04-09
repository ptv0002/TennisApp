using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Areas.NoRole.Models
{
    public class PasswordViewModel : DS_VDV
    {
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Không trùng mật khẩu!")]
        public string ConfirmPassword { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string CurrentAction { get; set; }
        public string CurrentController { get; set; }

    }
}