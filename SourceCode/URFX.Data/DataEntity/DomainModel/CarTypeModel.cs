using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity.DomainModel
{
   public class CarTypeModel
    {
        [Key]
        public int CarTypeId { get; set; }
        public string Description { get; set; }
    }
}
