namespace Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Thong_Bao
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        [DisplayName("T�n")]
        public string Ten { get; set; }
        [DisplayName("Ng�y")]
        public DateTime Ngay { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string File_Path { get; set; }
        public string File_Text { get; set; }
        public bool Hien_Thi { get; set; }
    }
}
