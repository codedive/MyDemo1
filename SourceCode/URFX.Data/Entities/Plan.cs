namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.Enums;

    public partial class Plan
    {   
        [Key]
        public int PlanId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Detail { get; set; }

        [Required]
        public decimal ApplicationFee { get; set; }

        [Required]
        public decimal TeamRegistrationFee { get; set; }

        [Required]
        [DefaultValue(0)]
        public TeamRegistrationType TeamRegistrationType { get; set; }

        [Required]
        public int PerVisitPercentage { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }        
    }
}
