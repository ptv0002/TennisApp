namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_Cap
    {
        public DS_Cap()
        {
            //DS_Diem = new HashSet<DS_Diem>();
            DS_Trans = new HashSet<DS_Tran>();
            
        }
        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        [DisplayName("Mã Cặp")]
        public string MaCap { get; set; }
        [DisplayName("Điểm")]
        public decimal? Diem { get; set; } // Total point to split
        public int? ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        [DisplayName("ID Trình")]
        public virtual DS_Trinh DS_Trinh { get; set; }

        public int ID_Bang { get; set; }
        [ForeignKey("ID_Bang")]
        [DisplayName("ID Bảng")]
        public virtual DS_Bang DS_Bang { get; set; }
        public int? ID_Vdv1 { get; set; }
        [ForeignKey("ID_Vdv1")]
        [DisplayName("ID VDV1")]
        public virtual DS_VDV VDV1 { get; set; }

        public int? ID_Vdv2 { get; set; }
        [ForeignKey("ID_Vdv2")]
        [InverseProperty("DS_Caps")]
        [DisplayName("ID VDV2")]
        public virtual DS_VDV VDV2 { get; set; }

        public virtual ICollection<DS_Tran> DS_Trans { get; set; }
    }
}
