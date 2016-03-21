namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmployeeServiceMapping")]
    public partial class EmployeeServiceMapping
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        public int? ServiceId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}
