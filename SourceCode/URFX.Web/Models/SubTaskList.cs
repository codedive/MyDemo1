using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Web.Models
{
    public class SubTaskList
    {
        public string Comments { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int ServiceId { get; set; }
      
        public List<PictureModel> Pictures { get; set; }

    }
}