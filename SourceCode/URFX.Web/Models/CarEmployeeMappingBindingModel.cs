﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class CarEmployeeMappingBindingModel
    {
        [Key]
        public int CarEmployeeMappingId { get; set; }
        public int CarTypeId { get; set; }
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("CarTypeId")]
        public virtual CarType CarType { get; set; }
    }
}