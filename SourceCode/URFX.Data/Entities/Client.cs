namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using URFX.Data.DataEntity;
   
    public partial class Client
    {     
        public Client()
        {                     
        }
        [Key]
        public string ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]        
        public string LastName { get; set; }

       

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(100)]
        public string NationalIdNumber { get; set; }

        [Required]
        [DefaultValue(1)]
        public int? NationaltIDType { get; set; }        

        [ForeignKey("ClientId")]
        public virtual ApplicationUser User { get; set; }
    }
}
