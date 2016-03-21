namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Enums;

    [Table("Complaint")]
    public partial class Complaint
    {
       
        public Complaint()
        {
            ComplaintImageMappings = new HashSet<ComplaintImageMapping>();
        }
        [Key]
        public int ComplaintId { get; set; }

        [StringLength(128)]
        public string ClientId { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public int? JobId { get; set; }

        public string Description { get; set; }

        public ComplaintStatus Status { get; set; }

        [StringLength(128)]
        public string ClosedBy { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }
        [ForeignKey("ServiceProviderId")]
        public virtual ServiceProvider ServiceProvider { get; set; }

        public virtual ICollection<ComplaintImageMapping> ComplaintImageMappings { get; set; }
    }
}
