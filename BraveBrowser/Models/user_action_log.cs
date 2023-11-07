namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_action_log
    {
        [Key]
        public long user_action_log_id { get; set; }

        public long from_user_id { get; set; }

        public long to_user_id { get; set; }

        [StringLength(500)]
        public string to_link { get; set; }

        [StringLength(50)]
        public string to_action { get; set; }

        public DateTime? date_created { get; set; }
    }
}
