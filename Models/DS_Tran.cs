namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class DS_Tran
    {
        public int Id { get; set; }

        [StringLength(15)]
        public string Ma_Tran { get; set; }

        public int? Kq_1 { get; set; }

        public int? Kq_2 { get; set; }

        //public int? ID_Cap1 { get; set; }

        //public int? ID_Cap2 { get; set; }
       // public int? Id_Vong { get; set; }

        [ForeignKey("ID_Cap1")]
        public virtual DS_Cap DS_Cap1 { get; set; }

        [ForeignKey("ID_Cap2")]
        [InverseProperty("DS_Trans")]
        public virtual DS_Cap DS_Cap2 { get; set; }

        [ForeignKey("ID_Vong")]
        public virtual DS_Vong DS_Vong { get; set; }

    }
}
