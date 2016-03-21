using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Enums;

namespace URFX.Data.DataEntity.DomainModel
{
  public  class ClientModel
    {

        public ClientModel()
        {
        }
        [Key]
        public string ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string OTP { get; set; }

        [StringLength(100)]
        public string ProfilePicturePath { get; set; }

        public string PhoneNumber { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }


        [DefaultValue(false)]
        public bool IsDeleted { get; set; }


        [StringLength(100)]
        public string NationalIdNumber { get; set; }
        public string DeviceType { get; set; }

        public string DeviceToken { get; set; }

        public string FacebookId { get; set; }

        public string GoogleId { get; set; }

        public string TwitterId { get; set; }

        public RegistrationType RegistrationType { get; set; }

        [DefaultValue(1)]
        public int? NationaltIDType { get; set; }

        public string QuickBloxId { get; set; }

        [DefaultValue(false)]
        public bool Registred { get; set; }

        [ForeignKey("ClientId")]
        public virtual ApplicationUser User { get; set; }
    }
}
