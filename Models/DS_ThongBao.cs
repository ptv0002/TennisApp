namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DS_ThongBao
    {
        [Key]
        public int Id { get; set; }

        [StringLength(80)]
        public string Ten { get; set; }

        public DateTime? Ngay { get; set; }

        public string FileTB { get; set; }
    }
}
