namespace Models
{
    using System;
    using System.Collections.Generic;
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
        public string MaCap { get; set; } 

        public decimal? Diem { get; set; } // Tổng điểm của cặp được phân bổ


        //public int Id_Trinh{ get; set; }
        [ForeignKey("ID_Trinh")]
        public virtual DS_Trinh DS_Trinh { get; set; }

        //public int? Id_Bang { get; set; }
        [ForeignKey("ID_Bang")]
        public virtual DS_Bang DS_Bang { get; set; }

        //public int? ID_Vdv1 { get; set; }
        [ForeignKey("ID_Vdv1")]
        public virtual DS_VDV VDV1 { get; set; }


        //public int? ID_Vdv2 { get; set; }

        [ForeignKey("ID_Vdv2")]
        [InverseProperty("DS_Caps")]
        public virtual DS_VDV VDV2 { get; set; }


        //[JsonIgnore]
        //public virtual ICollection<DS_Diem> DS_Diem { get; set; }
        public virtual ICollection<DS_Tran> DS_Trans { get; set; }
    }
}
