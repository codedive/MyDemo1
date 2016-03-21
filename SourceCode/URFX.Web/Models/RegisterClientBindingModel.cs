using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using URFX.Data.DataEntity;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Enums;

namespace URFX.Web.Models
{
    public class RegisterClientBindingModel
    {
        
        
        public string Email { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string ClientId { get; set; }

        
        [StringLength(100)]
        public string FirstName { get; set; }

       
        public string LastName { get; set; }

        public string OTP { get; set; }

        public string PhoneNumber { get; set; }

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

        public string DeviceType { get; set; }

        public string DeviceToken { get; set; }

        public string FacebookId { get; set; }

        public string GoogleId { get; set; }

        public string TwitterId { get; set; }

        public RegistrationType RegistrationType { get; set; }
        public int CityId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
        public string Address { get; set; }

        [ForeignKey("ClientId")]
        public virtual ApplicationUser User { get; set; }

        public decimal AverageRating { get; set; }
        public List<ClientRatingModel> ClientRatingModelList { get; set; }
        public UserLocationModel UserLocationModel { get; set; }

        public List<EmployeeModel> EmployeeModelList { get; set; }
    }
}