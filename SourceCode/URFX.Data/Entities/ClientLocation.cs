namespace URFX.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ClientLocation
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        [StringLength(100)]
        public string Address { get; set; }
        
        [ForeignKey("ClientId")]
        public string ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
