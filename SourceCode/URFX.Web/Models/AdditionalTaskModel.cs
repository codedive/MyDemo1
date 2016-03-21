using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Web.Models
{
    public class AdditionalTaskModel
    {
        public string Description { get; set; }
        public decimal? Cost { get; set; }

        public string Comment { get; set; }

        public int JobId { get; set; }

        
    }
}