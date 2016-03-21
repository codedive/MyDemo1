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
   public class ServiceProviderServiceMappingModel
    {
        public int Id { get; set; }

        public int ServiceId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int VisitRate { get; set; }

        public int HourlyRate { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        [StringLength(128)]
        public string ServiceProviderId { get; set; }
        [ForeignKey("ServiceProviderId")]
        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
