using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class RatingBindingModel
    {
        public int RatingId { get; set; }

        public int? JobId { get; set; }

        public string Comments { get; set; }


        public int OnTime { get; set; }


        public int Quality { get; set; }


        public int UnderStandingOfServiceRequired { get; set; }

        public int Cleanliness { get; set; }


        public int Communication { get; set; }


        public int Conduct { get; set; }

        public virtual Job Job { get; set; }
    }
}