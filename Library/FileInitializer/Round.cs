namespace Library.FileInitializer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Round
    {
        public int Ma_Vong { get; set; }
        [StringLength(20)]
        [DisplayName("Tên")]
        public string Ten { get; set; }
    }
}
