namespace Models
{
    using Newtonsoft.Json;
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
        [JsonIgnore]
        public virtual DS_Cap DS_Cap1 { get; set; }

        [ForeignKey("ID_Cap2")]
        [InverseProperty("DS_Trans")]
        [JsonIgnore]
        public virtual DS_Cap DS_Cap2 { get; set; }

        [ForeignKey("ID_Vong")]
        [JsonIgnore]
        public virtual DS_Vong DS_Vong { get; set; }

    }
}
