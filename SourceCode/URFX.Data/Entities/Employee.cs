namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;

    [Table("Employee")]
    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Complaints = new HashSet<Complaint>();
            EmployeeServiceMappings = new HashSet<EmployeeServiceMapping>();
            Jobs = new HashSet<Job>();
            Ratings = new HashSet<Rating>();
            ServicePoviderEmployeeMappings = new HashSet<ServicePoviderEmployeeMapping>();
        }

        public string EmployeeId { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [StringLength(150)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string ImagePath { get; set; }

        [StringLength(15)]
        public string CellNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CarLicensePlateNumber { get; set; }

        public int? EmployeeStatus { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual ApplicationUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmployeeServiceMapping> EmployeeServiceMappings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rating> Ratings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServicePoviderEmployeeMapping> ServicePoviderEmployeeMappings { get; set; }
    }
}
