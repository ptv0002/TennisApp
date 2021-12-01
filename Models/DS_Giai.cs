namespace Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public class DS_Giai
    {
        public DS_Giai()
        {
            DS_Trinh = new HashSet<DS_Trinh>();
        }

        [Key]
        public int Id { get; set; }

        [StringLength(120)]
        public string Ten { get; set; }

        public DateTime? Ngay { get; set; }
        [JsonIgnore]
        public virtual ICollection<DS_Trinh> DS_Trinh { get; set; }
    }
}
