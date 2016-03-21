using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity.DomainModel
{
    public class ServiceModel
    {        
        public int ServiceId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int VisitRate { get; set; }
        public int HourlyRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ServiceCategoryId { get; set; }
        public ServiceCategoryModel ServiceCategory { get; set; }
        public int? ParentServiceId { get; set; }        
    }
}
