using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class JobServiceMappingModel
    {
        public JobServiceMappingModel()
        {
            JobServicePictureMappings = new HashSet<JobServicePictureMappingModel>();
        }

        [Key]
        public int JobServiceMappingId { get; set; }

        public int? ServiceId { get; set; }

        public int? JobId { get; set; }

        public string Description { get; set; }

        public int? Quantity { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        public string ServiceName { get; set; }
        public virtual ICollection<JobServicePictureMappingModel> JobServicePictureMappings { get; set; }
    }
}
