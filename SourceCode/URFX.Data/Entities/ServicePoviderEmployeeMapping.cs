namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServicePoviderEmployeeMapping")]
    public partial class ServicePoviderEmployeeMapping
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
