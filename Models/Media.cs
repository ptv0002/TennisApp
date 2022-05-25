namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Media
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Menu")]
        public int Ma_Menu { get; set; }
        [DisplayName("Link thư mục hình")]
        public string Link_Hinh { get; set; }
        [DisplayName("Link thư mục video")]
        public string Link_Video { get; set; }
        public int ID_Giai { get; set; }
        [ForeignKey("ID_Giai")]
        public virtual DS_Giai DS_Giai { get; set; }
    }
}
