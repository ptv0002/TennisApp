namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class DS_Tran
    {
        public int Id { get; set; }

        [StringLength(15)]
        [DisplayName("Mã trận")]
        public string Ma_Tran { get; set; }
        [DisplayName("KQ 1")]
        public int Kq_1 { get; set; }
        [DisplayName("KQ 2")]
        public int Kq_2 { get; set; }
        public int? ID_Cap1 { get; set; }
        [ForeignKey("ID_Cap1")]
        public virtual DS_Cap DS_Cap1 { get; set; }
        [DisplayName("ID Cặp 2")]
        public int? ID_Cap2 { get; set; }
        [ForeignKey("ID_Cap2")]
        [InverseProperty("DS_Trans")]
        public virtual DS_Cap DS_Cap2 { get; set; }
        public int Ma_Vong { get; set; }
        public int ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        public virtual DS_Trinh DS_Trinh { get; set; }
    }
}
