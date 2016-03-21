using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Entities;
using URFX.Data.Enums;

namespace URFX.Web.Models
{
    public class RegisterServiceProviderBindingModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
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


        [StringLength(100)]
        public string RegistrationCertificatePath { get; set; }


        [StringLength(100)]
        public string GosiCertificatePath { get; set; }


        [DefaultValue(false)]
        public bool IsActive { get; set; }

        public DateTime? StartDate { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [StringLength(10)]
        public string FaxNumber { get; set; }
        public decimal AverageRating { get; set; }

        public ServiceProviderType ServiceProviderType { get; set; }


        public List<ServiceProviderServiceMappingModel> ServicesList { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public double Distance { get; set; }

        public List<EmployeeModel> EmployeesList { get; set; }

        public List<RatingModel> RatingModelList { get; set; }

        public UserLocationModel UserLocationModel { get; set; }

        public UserPlanModel UserPlan { get; set; }

        public PlanModel PlanModel { get; set; }
        public int CarTypeId { get; set; }
        public int TaskDone { get; set; }

        public bool Match { get; set; }
        public RatingModel RatingModel { get; set; }

        private string _ServiceProviderType = "";
        public string Type
        {
            get
            {
                _ServiceProviderType = Enum.GetName(typeof(ServiceProviderType), ServiceProviderType);
                return _ServiceProviderType;
            }
            set
            {
                value = _ServiceProviderType;

            }
        }
        private string _IsActiveStatus = "";
        public string IsActiveStatus
        {
            get
            {
                _IsActiveStatus = IsActive == true ? "Active" : "Deactive";
                return _IsActiveStatus;
            }
            set
            {
                value = _IsActiveStatus;

            }
        }
        public string UserName { get; set; }


    }
}