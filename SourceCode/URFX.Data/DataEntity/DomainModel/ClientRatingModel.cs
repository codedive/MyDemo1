﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class ClientRatingModel
    {
        public int ClientRatingId { get; set; }

        public int? JobId { get; set; }

        public string Comments { get; set; }

       
        public int Corporation { get; set; }
        public decimal TotalRating { get; set; }
        
        public int Communication { get; set; }

       
        public int UnderStanding { get; set; }

      

       
        public int Behaivor { get; set; }

       
        public int FriendLiness { get; set; }

        
        public int OverallSatisfaction { get; set; }

        public virtual Job Job { get; set; }
    }
}
