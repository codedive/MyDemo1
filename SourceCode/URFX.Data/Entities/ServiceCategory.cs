using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Entities
{
    public partial class ServiceCategory
    {
        [Key]
        public int ServiceCategoryId { get; set; }
        [StringLength(200)]
        [Required]
        public string Description { get; set; }
        public ICollection<Service> Services { get; set; }
    }
}
