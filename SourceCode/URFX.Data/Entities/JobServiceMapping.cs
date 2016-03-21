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
       
        public JobServiceMapping()
        {
            JobServicePicturesMappings = new HashSet<JobServicePicturesMapping>();
        }
        [Key]
        public int JobServiceMappingId { get; set; }

        public int? ServiceId { get; set; }

        public int? JobId { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        
        public virtual ICollection<JobServicePicturesMapping> JobServicePicturesMappings { get; set; }
    }
}
