namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class data_comment
    {
        [Key]
        public long data_comment_id { get; set; }

        public long data_post_id { get; set; }

        public string text_ai { get; set; }

        public string text_ai_en { get; set; }

        public long? crawl_comment_id { get; set; }

        public string crawl_text { get; set; }

        [StringLength(550)]
        public string crawl_link { get; set; }

        public DateTime? crawl_date { get; set; }

        [StringLength(250)]
        public string commented_link { get; set; }

        public bool is_delete { get; set; }

        public virtual data_post data_post { get; set; }
    }
}
