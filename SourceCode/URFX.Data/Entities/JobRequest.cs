namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JobRequest
    {
        public int JobRequestId { get; set; }

        public int JobId { get; set; }

        [Required]
        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public virtual Job Job { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
