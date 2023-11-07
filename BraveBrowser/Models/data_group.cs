namespace BraveBrowser.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class data_group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public data_group()
        {
            data_post = new HashSet<data_post>();
        }

        [Key]
        public long data_group_id { get; set; }

        [Required]
        [StringLength(250)]
        public string group_name { get; set; }

        [StringLength(550)]
        public string post_comment_url { get; set; }

        public bool is_delete { get; set; }

        [StringLength(50)]
        public string source { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<data_post> data_post { get; set; }
    }
}
