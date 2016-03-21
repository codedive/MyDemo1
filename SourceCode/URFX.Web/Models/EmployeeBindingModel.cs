using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Enums;

namespace URFX.Web.Models
{
    public class EmployeeBindingModel
    {
        public string EmployeeId { get; set; }
        public string ServiceProviderId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string ProfileImage { get; set; }

        public string NationalIdAndIqamaNumber { get; set; }

       

        public string LicensePlateNumber { get; set; }
        
        public string DeviceType { get; set; }
        
        public string DeviceToken { get; set; }

        public string QuickBloxID { get; set; }
        public bool Registred { get; set; }
        public int CarTypeId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public EmployeeStatus EmployeeStatus { get; set; }
        public UserLocationModel UserLocationModel { get; set; }

        public List<JobBindingModel> JobBindingModel { get; set; }

        public ServiceProviderModel model { get; set; }
    }
}