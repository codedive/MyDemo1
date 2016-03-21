using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class ServiceProviderEmployeeMappingModel
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        [StringLength(128)]
        public string ServiceProviderId { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ServiceProvider ServiceProvider { get; set; }
    }
}
