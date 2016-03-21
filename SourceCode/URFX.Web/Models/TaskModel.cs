using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Web.Models
{
    public class TaskModel
    {
        public string Description { get; set; }

        public string ClientId { get; set; }

        public string EmployeeId { get; set; }

        public string ServiceProviderId { get; set; }
        public int? Quantity { get; set; }
        public string Comments { get; set; }
        public int? ServiceId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Duration { get; set; }

        public decimal? Cost { get; set; }
        public string JobAddress { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string MerchantReference { get; set; }
        public List<SubTaskList> SubTaskList { get; set; }








    }
}