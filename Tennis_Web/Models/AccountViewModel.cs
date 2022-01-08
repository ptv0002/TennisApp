using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tennis_Web.Models
{
    public class AccountViewModel
    {
        public string Email { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Không trùng mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }
        [Display(Name = "Quyền sử dụng")]
        public string RoleName { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
    }
}
