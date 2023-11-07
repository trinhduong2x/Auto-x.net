namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [Key]
        public long user_id { get; set; }

        public long user_group_id { get; set; }

        [StringLength(50)]
        public string user_type { get; set; }

        [StringLength(250)]
        public string username { get; set; }

        [StringLength(250)]
        public string email { get; set; }

        [StringLength(250)]
        public string password { get; set; }

        [StringLength(50)]
        public string ip { get; set; }

        public DateTime? updated_date { get; set; }

        public DateTime? date_created { get; set; }

        public bool is_block { get; set; }

        public bool is_premium { get; set; }

        public bool is_delete { get; set; }
    }
}
