﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class ServiceCategoryModel
    {
        public int ServiceCategoryId { get; set; }
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        public ICollection<Service> Services { get; set; }
        [StringLength(100)]
        public string CategoryPicturePath { get; set; }
    }
}
