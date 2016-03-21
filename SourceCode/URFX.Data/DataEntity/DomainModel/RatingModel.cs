using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class RatingModel
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

       public decimal TotalRating { get; set; }

        public virtual Job Job { get; set; }



    }
}
