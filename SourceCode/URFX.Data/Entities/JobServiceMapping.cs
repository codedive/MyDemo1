namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobServiceMapping")]
    public partial class JobServiceMapping
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobServiceMapping()
        {
            JobServicePicturesMappings = new HashSet<JobServicePicturesMapping>();
        }

        public int JobServiceMappingId { get; set; }

        public int? ServiceId { get; set; }

        public int? JobId { get; set; }

        public int? Quantity { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }

        public virtual Job Job { get; set; }

        public virtual Service Service { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServicePicturesMapping> JobServicePicturesMappings { get; set; }
    }
}
