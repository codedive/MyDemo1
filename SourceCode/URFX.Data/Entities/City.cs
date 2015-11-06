using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Entities
{    
    public partial class City
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
