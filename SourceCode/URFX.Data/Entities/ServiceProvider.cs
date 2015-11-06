namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;

    [Table("ServiceProvider")]
    public partial class ServiceProvider
    {
        public ServiceProvider()
        {
            
        }
        [Key]
        public string ServiceProviderId { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyRegistrationNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string GeneralManagerName { get; set; }

        [StringLength(100)]
        public string CompanyLogoPath { get; set; }

        [Required]
        [StringLength(100)]
        public string RegistrationCertificatePath { get; set; }

        [Required]
        [StringLength(100)]
        public string GosiCertificatePath { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
               
        [StringLength(10)]
        public string FaxNumber { get; set; }         

        [DefaultValue(0.00)]
        public decimal MinimumServicePrice { get; set; }
        public string Location { get; set; }
                
        [ForeignKey("ServiceProviderId")]
        public virtual ApplicationUser User { get; set; }

        

    }
}
