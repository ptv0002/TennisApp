namespace Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_VDV
    {
        public DS_VDV()
        {
            DS_Caps = new HashSet<DS_Cap>();
            DS_VDV_Diem = new HashSet<DS_VDVDiem>();
        }
        [Key]
        public int Id { get; set; }

        [StringLength(25)]
        [DisplayName("Họ và tên")]
        public string Ho_Ten { get; set; }

        [StringLength(30)]
        [DisplayName("Tên Tắt")]
        public string Ten_Tat { get; set; }
        [DisplayName("Dữ liệu cần phê duyệt")]
        public string Data_PD { get; set; }
        [DisplayName("Phê duyệt")]
        public bool? Phe_Duyet { get; set; }
        [DisplayName("Tham gia")]
        public bool Tham_Gia { get; set; }
        [DisplayName("Giới tính")]
        public bool Gioi_Tinh { get; set; }

        [StringLength(40)]
        public string CLB { get; set; }
        [DisplayName("Khách mời")]
        public bool Khach_Moi { get; set; }

        [NotMapped]
        public IFormFile Picture { get; set; }
        [NotMapped]
        public string File_Anh { get; set; }

        [DisplayName("SĐT")]
        public string Tel { get; set; }

        [StringLength(40)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [StringLength(20)]
        public string Password { get; set; }
        [DisplayName("Không hoạt động")]
        public bool Status { get; set; }

        [DisplayName("Điểm")]
        public int Diem { get; set; }

        [DisplayName("Điểm Cũ")]
        public int Diem_Cu { get; set; }

        [DisplayName("Công Ty")]
        public string Cong_Ty { get; set; }

        [DisplayName("Chức Vụ")]
        public string Chuc_Vu { get; set; }

        [ForeignKey("ID_KhuVuc")]
        [DisplayName("ID Khu Vực")]
        public virtual Khu_Vuc Khu_Vuc { get; set; }
        public virtual ICollection<DS_Cap> DS_Caps { get; set; }
        public virtual ICollection<DS_VDVDiem> DS_VDV_Diem { get; set; }
    }
}
