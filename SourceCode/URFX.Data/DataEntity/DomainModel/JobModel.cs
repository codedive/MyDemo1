using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;
using URFX.Data.Enums;

namespace URFX.Data.DataEntity.DomainModel
{
   public class JobModel
    {
        public JobModel()
        {
            Complaints = new HashSet<Complaint>();
            EmployeeSchedules = new HashSet<EmployeeSchedule>();
            JobPayments = new HashSet<JobPayment>();
            JobRequests = new HashSet<JobRequest>();
            JobServiceMappings = new HashSet<JobServiceMapping>();
            Ratings = new HashSet<Rating>();
        }
        [Key]
        public int JobId { get; set; }

        public string Description { get; set; }

        public string AdditionalTaskDescription { get; set; }

        [StringLength(128)]
        public string ClientId { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public JobStatus Status { get; set; }
        public string Comment { get; set; }
        public int? Type { get; set; }

        public int? ServiceId { get; set; }
        public int? Quantity { get; set; }
        public string Comments { get; set; }
        public bool IsPaid { get; set; }
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
        public string JobAddress { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
        public decimal? Cost { get; set; }

        [StringLength(128)]
        public string CancelledBy { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


        public virtual ICollection<Complaint> Complaints { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }


        public virtual ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }
        [ForeignKey("ServiceProviderId")]
        public virtual ServiceProvider ServiceProvider { get; set; }


        public virtual ICollection<JobPayment> JobPayments { get; set; }


        public virtual ICollection<JobRequest> JobRequests { get; set; }


        public virtual ICollection<JobServiceMapping> JobServiceMappings { get; set; }


        public virtual ICollection<Rating> Ratings { get; set; }

        public List<TransactionHistoryModel> TransactionHistoryModel { get; set; }

        public ClientModel ClientModel { get; set; }

    }
}
