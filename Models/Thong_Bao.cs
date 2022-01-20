namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Thong_Bao
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        [DisplayName("T�n")]
        public string Ten { get; set; }
        [DisplayName("Ng�y")]
        public DateTime Ngay { get; set; }
        public string File_TB { get; set; }
        public bool Hien_Thi { get; set; }
    }
}
