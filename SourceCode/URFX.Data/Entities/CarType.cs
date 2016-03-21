using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Entities
{
    [Table("CarType")]
   public class CarType
    {
        [Key]
        public int CarTypeId { get; set; }
        public string Description { get; set; }

        
    }
}
