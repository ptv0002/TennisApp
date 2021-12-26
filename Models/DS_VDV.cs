namespace Models
{
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
            //DS_Cap2 = new HashSet<DS_Cap>();
        }
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [DisplayName("Họ")]
        public string Ho { get; set; }

        [StringLength(7)]
        [DisplayName("Tên")]
        public string Ten { get; set; }

        [StringLength(40)]
        [DisplayName("Tên Tắt")]
        public string Ten_Tat { get; set; }

        public bool? Tham_Gia { get; set; }

        [StringLength(40)]
        public string CLB { get; set; }

        public bool? KhachMoi { get; set; }


        public string FileAnh { get; set; }

        [DisplayName("SĐT")]
        public string Tel { get; set; }

        [StringLength(40)]
        public string Email { get; set; }

        public bool? Status { get; set; }

        [DisplayName("Điểm")]
        public int? Diem { get; set; }

        [DisplayName("Điểm Cũ")]
        public int? DiemCu { get; set; }

        [DisplayName("Công Ty")]
        public string CongTy { get; set; }

        [DisplayName("Chức Vụ")]
        public string ChucVu { get; set; }

        //[DisplayName("Tham Gia")]
        //public bool? ThamGia { get; set; }
        //public int? Id_KhuVuc { get; set; }
        [ForeignKey("ID_KhuVuc")]
        [DisplayName("ID Khu Vực")]
        public virtual Khu_Vuc Khu_Vuc { get; set; }
        public virtual ICollection<DS_Cap> DS_Caps { get; set; }
     //  [JsonIgnore]
     //   public virtual ICollection<DS_Cap> DS_Cap2 { get; set; }

    }
}
