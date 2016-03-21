using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
    public class UserPlanModel
    {
        public int Id { get; set; }
        public int PlanId { get; set; }       
        public string UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }       
        public ApplicationUser AspNetUser { get; set; }        
        public PlanModel Plan { get; set; }
        public int NumberOfTeams { get; set; }
    }
}
