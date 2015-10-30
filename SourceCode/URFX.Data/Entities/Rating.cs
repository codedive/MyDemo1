namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rating")]
    public partial class Rating
    {
        public int RatingId { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        public int? JobId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        [StringLength(10)]
        public string OnTime { get; set; }

        [StringLength(10)]
        public string Quality { get; set; }

        [StringLength(10)]
        public string UnderStandingOfServiceRequired { get; set; }

        [StringLength(10)]
        public string Cleanliness { get; set; }

        [StringLength(10)]
        public string Communication { get; set; }

        [StringLength(10)]
        public string Conduct { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
