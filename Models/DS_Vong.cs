namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_Vong
    {
        public DS_Vong()
        {
        //    DS_Diem = new HashSet<DS_Diem>();
        //    DS_Tran = new HashSet<DS_Tran>();

        }

        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [DisplayName("Tên")]
        public string Ten { get; set; }
        [StringLength(1)]
        [DisplayName("Mã Vòng")]
        public string MaVong { get; set; }
        //public virtual ICollection<DS_Diem> DS_Diem { get; set; }
        //public virtual ICollection<DS_Tran> DS_Tran { get; set; }
    }
}
