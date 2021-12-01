namespace Models
{
    using System;
    using System.Collections.Generic;
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
        public string Ho { get; set; }

        [StringLength(7)]
        public string Ten { get; set; }

        [StringLength(25)]
        public string Ten_Tat { get; set; }

        [StringLength(40)]
        public string CLB { get; set; }

        public bool? KhachMoi { get; set; }


        public string FileAnh { get; set; }

        public string Tel { get; set; }

        [StringLength(40)]
        public string Email { get; set; }

        public bool? Status { get; set; }

        public int? Diem { get; set; }

        public int? DiemCu { get; set; }

        public string CongTy { get; set; }

        public string ChucVu { get; set; }

        //public int? Id_KhuVuc { get; set; }
        [ForeignKey("ID_KhuVuc")]
        public virtual Khu_Vuc Khu_Vuc { get; set; }
        public virtual ICollection<DS_Cap> DS_Caps { get; set; }
     //  [JsonIgnore]
     //   public virtual ICollection<DS_Cap> DS_Cap2 { get; set; }

    }
}
