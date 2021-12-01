namespace Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
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
        public decimal? Diem { get; set; } // Tổng điểm dùng phân bổ cho bảng
        public string Ten { get; set; }
        //public int? ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        [JsonIgnore]
        public virtual DS_Trinh DS_Trinh { get; set; }

        [JsonIgnore]
        public virtual ICollection<DS_Cap> DS_Cap { get; set; }
    }
 }
