namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class DS_Bang
    {
        public DS_Bang()
        {
            DS_Cap = new HashSet<DS_Cap>();
        }
        [Key]
        public int Id { get; set; }
        [DisplayName("Điểm")]
        public decimal? Diem { get; set; } // Total point to split
        [DisplayName("Tên")]
        public string Ten { get; set; }
        public int ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        [DisplayName("ID Trình")]
        public virtual DS_Trinh DS_Trinh { get; set; }
        public virtual ICollection<DS_Cap> DS_Cap { get; set; }
    }
 }
