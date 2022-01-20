namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_Diem  // Danh sách điểm được phân bổ các vòng cho các cặp
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Điểm")]
        public decimal Diem { get; set; }
        public int ID_Cap { get; set; }
        [ForeignKey("ID_Cap")]
        [DisplayName("ID Cặp")]
        public virtual DS_Cap DS_Cap { get; set; }
        public int ID_Vong { get; set; }
        [ForeignKey("ID_Vong")]
        [DisplayName("ID Vòng")]
        public virtual DS_Vong DS_Vong { get; set; }
        public int ID_Trinh { get; set; }
        [ForeignKey("ID_Trinh")]
        [DisplayName("ID Trình")]
        public virtual DS_Trinh DS_Trinh { get; set; }

    }
}
