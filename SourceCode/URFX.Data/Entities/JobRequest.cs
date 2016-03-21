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

      
        [StringLength(128)]
        public string ServiceProviderId { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        [ForeignKey("ServiceProviderId")]
        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
