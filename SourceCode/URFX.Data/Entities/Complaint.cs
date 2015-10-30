namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Complaint")]
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            ComplaintImageMappings = new HashSet<ComplaintImageMapping>();
        }

        public int ComplaintId { get; set; }

        [StringLength(128)]
        public string ClientId { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public int? JobId { get; set; }

        public string Description { get; set; }

        public int? Status { get; set; }

        [StringLength(128)]
        public string ClosedBy { get; set; }

        public virtual Client Client { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComplaintImageMapping> ComplaintImageMappings { get; set; }
    }
}
