using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.DataEntity;

namespace URFX.Data.Entities
{
    [Table("UserLocation")]
   public partial class UserLocation
    {
        [Key]
        public int UserLocationId { get; set; }

        public int CityId { get; set; }
        
        public string Address { get; set; }

        public int DistrictId { get; set; }
        
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
