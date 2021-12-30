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
        [DisplayName("Mã Trận")]
        public string Ma_Tran { get; set; }
        [DisplayName("KQ1")]
        public int? Kq_1 { get; set; }
        [DisplayName("KQ2")]
        public int? Kq_2 { get; set; }
        public int? ID_Cap1 { get; set; }
        [DisplayName("ID Cặp 1")]
        [ForeignKey("ID_Cap1")]
        public virtual DS_Cap DS_Cap1 { get; set; }
        public int? ID_Cap2 { get; set; }
        [DisplayName("ID Cặp 2")]
        [ForeignKey("ID_Cap2")]
        [InverseProperty("DS_Trans")]
        public virtual DS_Cap DS_Cap2 { get; set; }
        public int ID_Vong { get; set; }
        [DisplayName("ID Vòng")]
        [ForeignKey("ID_Vong")]
        public virtual DS_Vong DS_Vong { get; set; }

    }
}
