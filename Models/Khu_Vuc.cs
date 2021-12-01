namespace Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Khu_Vuc
    {
        public Khu_Vuc()
        {
            DS_VDV = new HashSet<DS_VDV>();
        }

        [Key]
        public int Id { get; set; }

        public string MaKhuVuc { get; set; }

        public string Ten { get; set; }

        [JsonIgnore]
        public virtual ICollection<DS_VDV> DS_VDV { get; set; }
    }
}
