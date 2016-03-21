using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class JobServiceMappingBindingModel
    {
        public JobServiceMappingBindingModel()
        {
            JobServicePicturesMappings = new HashSet<JobServicePicturesMapping>();
        }
        public int JobServiceMappingId { get; set; }

        public int? ServiceId { get; set; }

        public int? JobId { get; set; }

        public int? Quantity { get; set; }

        
        public string Comments { get; set; }
      
        public virtual Job Job { get; set; }
      
        public virtual Service Service { get; set; }

        public virtual ICollection<JobServicePicturesMapping> JobServicePicturesMappings { get; set; }
    }
}