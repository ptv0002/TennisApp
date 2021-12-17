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
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Mã Khu Vực")]
        public string MaKhuVuc { get; set; }
        [DisplayName("Tên")]
        public string Ten { get; set; }
        public virtual ICollection<DS_VDV> DS_VDV { get; set; }
    }
}
