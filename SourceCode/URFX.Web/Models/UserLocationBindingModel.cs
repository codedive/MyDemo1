using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using URFX.Data.DataEntity;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class UserLocationBindingModel
    {
        [Key]
        public int UserLocationId { get; set; }

        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }

        public int DistrictId { get; set; }
        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}