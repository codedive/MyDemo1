namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Job")]
    public partial class Job
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            Complaints = new HashSet<Complaint>();
            EmployeeSchedules = new HashSet<EmployeeSchedule>();
            JobPayments = new HashSet<JobPayment>();
            JobRequests = new HashSet<JobRequest>();
            JobServiceMappings = new HashSet<JobServiceMapping>();
            Ratings = new HashSet<Rating>();
        }

        public int JobId { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string ClientId { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Status { get; set; }

        public int? Type { get; set; }

        public bool IsRated { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CompletionCode { get; set; }

        public bool IsComplete { get; set; }

        [StringLength(128)]
        public string ConfirmedBy { get; set; }

        public bool IsCompleteConfimed { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(128)]
        public string ClosedBy { get; set; }

        public DateTime? ClosedDate { get; set; }

        public int? Duration { get; set; }

        public decimal? Cost { get; set; }

        [StringLength(128)]
        public string CancelledBy { get; set; }

        public virtual Client Client { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }

        public virtual Employee Employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobPayment> JobPayments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobRequest> JobRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobServiceMapping> JobServiceMappings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
