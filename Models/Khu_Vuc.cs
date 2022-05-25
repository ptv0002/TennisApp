namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Khu_Vuc
    {
        public Khu_Vuc()
        {
            DS_VDV = new HashSet<DS_VDV>();
            DS_Giai = new HashSet<DS_Giai>();
            Thong_Baos = new HashSet<Thong_Bao>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Mã khu vực")]
        public string Ma_KhuVuc { get; set; }
        [DisplayName("Tên")]
        public string Ten { get; set; }
        public virtual ICollection<DS_VDV> DS_VDV { get; set; }
        public virtual ICollection<DS_Giai> DS_Giai { get; set; }
        public virtual ICollection<Thong_Bao> Thong_Baos { get; set; }
    }
}
