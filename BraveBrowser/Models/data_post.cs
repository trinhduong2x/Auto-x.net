namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class data_post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public data_post()
        {
            data_comment = new HashSet<data_comment>();
        }

        [Key]
        public long data_post_id { get; set; }

        public long data_group_id { get; set; }

        public string image { get; set; }

        public string text_ai { get; set; }

        public string text_ai_en { get; set; }

        [StringLength(250)]
        public string posted_link { get; set; }

        public long? crawl_post_id { get; set; }

        public string crawl_text { get; set; }

        [StringLength(550)]
        public string crawl_link { get; set; }

        public DateTime? crawl_date { get; set; }

        public bool is_delete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<data_comment> data_comment { get; set; }

        public virtual data_group data_group { get; set; }
    }
}
