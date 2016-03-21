using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;
using URFX.Data.Enums;

namespace URFX.Data.DataEntity.DomainModel
{
    public class ComplaintModel
    {
        public int ComplaintId { get; set; }
        public string Description { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public string JobId { get; set; }
        public string JobDescription { get; set; }
        public ComplaintStatus Status { get; set; }
        public string ClosedBy { get; set; }

        public string JobAddress { get; set; }

        public string ClientAddress { get; set; }

        public string ClientPhoneNumber { get; set; }
        public virtual Job Job { get; set; }

    }
}
