namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_VDVDiem
    {
    [Key]
        public int Id { get; set; }

        public int ID_Giai { get; set; }
        [DisplayName("Ngày")]
        public DateTime Ngay { get; set; }
        [DisplayName("Điểm")]
        public int Diem { get; set; }
        [DisplayName("ID VĐV")]
        public int ID_Vdv { get; set; }
        [ForeignKey("ID_Vdv")]
        public virtual DS_VDV DS_VDV { get; set; }

    }
}
