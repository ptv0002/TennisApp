namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Hoat_Dong
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Link tới album hình")]
        public string Link_Hinh { get; set; }
        [DisplayName("Link tới album video")]
        public string Link_Video { get; set; }
    }
}
