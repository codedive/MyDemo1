using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class JobServicePictureMappingModel
    {
        public int Id { get; set; }

        
        public string ImagePath { get; set; }

        public int? JobServiceMappingId { get; set; }
        
        public virtual JobServiceMapping JobServiceMapping { get; set; }
    }
}
