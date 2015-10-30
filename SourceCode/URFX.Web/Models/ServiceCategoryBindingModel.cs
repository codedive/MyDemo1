using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class ServiceCategoryBindingModel
    {
        public int ServiceCategoryId { get; set; }
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}