namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_Trinh
    {
        public DS_Trinh()
        {
            DS_Cap = new HashSet<DS_Cap>();
            DS_Bang = new HashSet<DS_Bang>();
        }
        [Key]
        public int Id { get; set; }
        [DisplayName("Trình")]
        public int Trinh { get; set; }
        [DisplayName("Tỉ lệ Vô Địch")]
        public decimal TL_VoDich { get; set; }
        [DisplayName("Tỉ lệ Chung Kết")]
        public decimal TL_ChungKet { get; set; }
        [DisplayName("Tỉ lệ Bán Kết")]
        public decimal TL_BanKet { get; set; }
        [DisplayName("Tỉ lệ Tứ Kết")]
        public decimal TL_TuKet { get; set; }
        [DisplayName("Tỉ lệ Bảng")]
        public decimal TL_Bang { get; set; } // T_%TP
        [DisplayName("Hệ số phân bổ")]
        public int Diem_PB { get; set; }  // T_PB
        [DisplayName("Tổng điểm")]
        public int Tong_Diem { get; set; } // T_DT
        [DisplayName("Điểm trích cơ bản")]
        public int Diem_Tru { get; set; } // T_TD
        [DisplayName("Chênh lệch")]
        public int Chenh_Lech { get; set; }
        public int ID_Giai { get; set; }
        [ForeignKey("ID_Giai")]
        public virtual DS_Giai DS_Giai { get; set; }
        public virtual ICollection<DS_Tran> DS_Tran { get; set; }
        public virtual ICollection<DS_Cap> DS_Cap { get; set; }
        public virtual ICollection<DS_Bang> DS_Bang { get; set; }
    }
}
