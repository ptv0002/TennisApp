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
            DS_Trans = new HashSet<DS_Tran>();
        }
        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        [DisplayName("Mã cặp")]
        public string Ma_Cap { get; set; }
        [DisplayName("Điểm cặp")]
        public int Diem { get; set; } // Total point from 2 players
        [DisplayName("Số trận thắng")]
        public int Tran_Thang { get; set; }
        [DisplayName("Bốc thăm")]
        public int Boc_Tham { get; set; }
        [DisplayName("Xếp hạng")]
        public int Xep_Hang { get; set; }
        public int ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        public virtual DS_Trinh DS_Trinh { get; set; }
        public int? ID_Bang { get; set; }
        [ForeignKey("ID_Bang")]
        public virtual DS_Bang DS_Bang { get; set; }
        public int ID_Vdv1 { get; set; }
        [ForeignKey("ID_Vdv1")]
        public virtual DS_VDV VDV1 { get; set; }

        public int? ID_Vdv2 { get; set; }
        [ForeignKey("ID_Vdv2")]
        [InverseProperty("DS_Caps")]
        public virtual DS_VDV VDV2 { get; set; }

        public virtual ICollection<DS_Tran> DS_Trans { get; set; }
        public int Hieu_so { get; set; }
        [NotMapped]
        public int Tinh_toan { get; set; }
    }
}
