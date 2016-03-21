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

    public partial class Client
    {     
        public Client()
        {                     
        }
        [Key]
        public string ClientId { get; set; }

       
        [StringLength(100)]
        public string FirstName { get; set; }

            
        public string LastName { get; set; }

        public string OTP { get; set; }

        [StringLength(100)]
        public string ProfilePicturePath { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

       
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        
        [StringLength(100)]
        public string NationalIdNumber { get; set; }

       
        [DefaultValue(1)]
        public int? NationaltIDType { get; set; }      
        
        public string QuickBloxId { get; set; }

        [DefaultValue(false)]
        public bool Registred { get; set; }

        [ForeignKey("ClientId")]
        public virtual ApplicationUser User { get; set; }

        
    }
}
