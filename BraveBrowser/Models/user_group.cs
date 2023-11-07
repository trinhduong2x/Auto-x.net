namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_group
    {
        [Key]
        public long user_group_id { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [StringLength(50)]
        public string code { get; set; }

        [StringLength(50)]
        public string alias { get; set; }

        public int? sort_order { get; set; }

        public DateTime? updated_date { get; set; }
    }
}
