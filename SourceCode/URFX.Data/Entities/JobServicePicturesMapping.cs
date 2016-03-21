namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobServicePicturesMapping")]
    public partial class JobServicePicturesMapping
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string ImagePath { get; set; }

        public int? JobServiceMappingId { get; set; }
        [ForeignKey("JobServiceMappingId")]
        public virtual JobServiceMapping JobServiceMapping { get; set; }
    }
}
