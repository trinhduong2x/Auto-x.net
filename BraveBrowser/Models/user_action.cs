namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_action
    {
        [Key]
        public long user_action_id { get; set; }

        public long user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string action_name { get; set; }

        [Required]
        [StringLength(50)]
        public string action_code { get; set; }

        [Required]
        [StringLength(50)]
        public string action_daily_setting { get; set; }

        public int action_number { get; set; }

        public DateTime? created_date { get; set; }

        public bool is_active { get; set; }
    }
}
