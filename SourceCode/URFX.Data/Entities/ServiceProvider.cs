namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;
    using Enums;

    [Table("ServiceProvider")]
    public partial class ServiceProvider
    {
        public ServiceProvider()
        {
            
        }
        [Key]
        public string ServiceProviderId { get; set; }

        
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

      
        [StringLength(100)]
        public string CompanyRegistrationNumber { get; set; }

       
        [StringLength(100)]
        public string GeneralManagerName { get; set; }

        [StringLength(100)]
        public string CompanyLogoPath { get; set; }

       
        [StringLength(100)]
        public string RegistrationCertificatePath { get; set; }

       
        [StringLength(100)]
        public string GosiCertificatePath { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public DateTime? StartDate { get; set; }

        
        public string FaxNumber { get; set; }         

        public ServiceProviderType ServiceProviderType { get; set; }
        public string Location { get; set; }
                
        [ForeignKey("ServiceProviderId")]
        public virtual ApplicationUser User { get; set; }

        

    }
}
