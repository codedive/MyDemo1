namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    
    public partial class Service
    {   
        [Key]
        public int ServiceId { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }
              
        [Required]
        public decimal Price { get; set; }  
       
        public int  VisitRate { get; set; }
       
        public int HourlyRate { get; set; }

        public bool IsActive { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public int? ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public ServiceCategory ServiceCategory { get; set; }

        
        public int? ParentServiceId { get; set; }

        
    }
}
