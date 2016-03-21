using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Enums;

namespace URFX.Data.DataEntity.DomainModel
{
    public class ServiceProviderModel
    {
        public string ServiceProviderId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string Description { get; set; }

        public string CompanyRegistrationNumber { get; set; }

        [Required]
        public string GeneralManagerName { get; set; }
        public DateTime? StartDate { get; set; }

        public string CompanyLogoPath { get; set; }


        public string RegistrationCertificatePath { get; set; }


        public string GosiCertificatePath { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }


        public string PhoneNumber { get; set; }


        public string FaxNumber { get; set; }

        public ServiceProviderType ServiceProviderType { get; set; }
        public List<ServiceProviderServiceMappingModel> ServicesList { get; set; }

        public List<EmployeeModel> EmployeesList { get; set; }

        public List<RatingModel> RatingModelList { get; set; }

        public UserLocationModel UserLocationModel { get; set; }
    }
}
