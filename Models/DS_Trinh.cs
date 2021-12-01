namespace Models
{
    using System;
    using System.Collections.Generic;
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

        public int? Trinh { get; set; }

        public int? TongDiem { get; set; }

        public int? DiemTru { get; set; }

        public int? ChenhLech { get; set; }

        //public int? Id_Giai { get; set; }
        [ForeignKey("ID_Giai")]
        public virtual DS_Giai DS_Giai { get; set; }
        public virtual ICollection<DS_Cap> DS_Cap { get; set; }
        public virtual ICollection<DS_Bang> DS_Bang { get; set; }

    }
}
