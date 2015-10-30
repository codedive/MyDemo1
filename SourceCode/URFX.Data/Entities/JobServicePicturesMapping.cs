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
        public int Id { get; set; }

        [StringLength(100)]
        public string ImagePath { get; set; }

        public int? JobServiceMappingId { get; set; }

        public virtual JobServiceMapping JobServiceMapping { get; set; }
    }
}
