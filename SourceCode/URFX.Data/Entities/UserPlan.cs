namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;
        
    public partial class UserPlan
    {
        [Key]
        public int Id { get; set; }

        public int PlanId { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public int NumberOfTeams { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser AspNetUser { get; set; }
        [ForeignKey("PlanId")]
        public virtual Plan Plan { get; set; }
    }
}
