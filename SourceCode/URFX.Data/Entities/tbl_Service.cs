using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Entities
{
  public  class tbl_Service
    {
        [Key]
        public int ServiceId { get; set; }

        public string Description { get; set; }

        public int Duration { get; set; }

        public decimal Cost { get; set; }

        public int ParentServiceId { get; set; }

       
    }
}
