﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
    }
}