namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComplaintImageMapping")]
    public partial class ComplaintImageMapping
    {
        [Key]
        public int ComplaintImageId { get; set; }

        public int? ComplaintId { get; set; }

        [StringLength(100)]
        public string ComplaintImagePath { get; set; }

        public virtual Complaint Complaint { get; set; }
    }
}
