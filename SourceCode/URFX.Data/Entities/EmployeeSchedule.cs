namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeSchedule")]
    public partial class EmployeeSchedule
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string Description { get; set; }

        public int? JobId { get; set; }

        public virtual Employee Employee { get; set; }       

        public virtual Job Job { get; set; }
    }
}
