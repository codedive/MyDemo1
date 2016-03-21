using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Enums;

namespace URFX.Web.Models
{
    public class PlanBindingModel
    {
        public int PlanId { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public decimal ApplicationFee { get; set; }
        public decimal TeamRegistrationFee { get; set; }
        public TeamRegistrationType TeamRegistrationType { get; set; }
        public int? PerVisitPercentage { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
