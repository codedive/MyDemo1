﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class ServiceProviderServiceMappingBindingModel
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int VisitRate { get; set; }

        public int HourlyRate { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        [StringLength(128)]
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public virtual ServiceProvider ServiceProvider { get; set; }

        public int CategoryId { get; set; }

    }
}