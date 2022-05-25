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
        [DisplayName("Tên")]
        public string Ten { get; set; }
        [DisplayName("Ngày")]
        public DateTime Ngay { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string File_Path { get; set; }
        [DisplayName("Soạn thảo thông báo")]
        public string File_Text { get; set; }
        [DisplayName("Hiển thị")]
        public bool Hien_Thi { get; set; }
        [DisplayName("Tin nổi bật")]
        public bool Tin_Nong { get; set; }
        public int? ID_Giai { get; set; }
        [ForeignKey("ID_Giai")]
        public virtual DS_Giai DS_Giai { get; set; }
        public int ID_KhuVuc { get; set; }
        [ForeignKey("ID_KhuVuc")]
        public virtual Khu_Vuc Khu_Vucs { get; set; }
    }
}
