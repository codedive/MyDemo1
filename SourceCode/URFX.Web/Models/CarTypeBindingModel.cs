using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URFX.Web.Models
{
    public class CarTypeBindingModel
    {
        [Key]
        public int CarTypeId { get; set; }
        public string Description { get; set; }
    }
}