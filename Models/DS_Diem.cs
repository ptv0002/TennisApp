namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_Diem  // Danh sách điểm được phân bổ các vòng cho các cặp
    {
        [Key]
        public int Id { get; set; }

        public decimal? Diem { get; set; }

        //public int? Id_Cap { get; set; } // Mã cặp = null trống tương ứng với Điểm thưởng cho Trình

        //public int? Id_Vong { get; set; }
        //public int? Id_Trinh { get; set; }

        [ForeignKey("ID_Cap")]
        public virtual DS_Cap DS_Cap { get; set; }
        [ForeignKey("ID_Vong")]
        public virtual DS_Vong DS_Vong { get; set; }
        [ForeignKey("ID_Trinh")]
        public virtual DS_Trinh DS_Trinh { get; set; }

    }
}
