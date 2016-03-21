using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URFX.Data.Entities;

namespace URFX.Web.Models
{
    public class ComplaintBindingModel
    {
        public int ComplaintId { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public int JobId { get; set; }
        public string JobDescription { get; set; }
        public int Status { get; set; }
        public string ClosedBy { get; set; }
        public string JobAddress { get; set; }

        public string ClientAddress { get; set; }

        public string ClientPhoneNumber { get; set; }
        public virtual Job Job { get; set; }
    }
}