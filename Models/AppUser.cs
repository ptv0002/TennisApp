using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Models {
    public class AppUser : IdentityUser {
        [MaxLength (30)]
        public string FullName { set; get; }

        [MaxLength(100)]
        public string Ghi_Chu { set; get; }
    }
}