namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class DS_Giai
    {
        public DS_Giai()
        {
            DS_Trinh = new HashSet<DS_Trinh>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName("Giải Mới")]
        public bool Giai_Moi { get; set; }
        [StringLength(120)]
        [DisplayName("Tên")]
        public string Ten { get; set; }
        [DisplayName("Ngày")]
        public DateTime Ngay { get; set; }
        [DisplayName("Ghi Chú")]
        public string Ghi_Chu { get; set; }
        public int? ID_KhuVuc { get; set; }
        [ForeignKey("ID_KhuVuc")]
        public virtual Khu_Vuc Khu_Vucs { get; set; }
        public virtual ICollection<DS_Trinh> DS_Trinh { get; set; }
    }
}
