namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("JobPayment")]
    public partial class JobPayment
    {
        public int JobPaymentId { get; set; }

        [StringLength(128)]
        public string ClientId { get; set; }

        public int? JobId { get; set; }

        public decimal? Amount { get; set; }

        public decimal? RefundedAmount { get; set; }

        [StringLength(128)]
        public string RefundedBy { get; set; }

        public string RefundReason { get; set; }

        public virtual Client Client { get; set; }

        public virtual Job Job { get; set; }
    }
}
