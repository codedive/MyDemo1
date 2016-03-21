namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;

    [Table("PlanPayment")]
    public partial class PlanPayment
    {
        [Key]
        public int PlanPaymentId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int? PlanId { get; set; }

        public decimal? Amount { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUser { get; set; }

        [ForeignKey("PlanId")]
        public virtual Plan Plan { get; set; }
    }
}
