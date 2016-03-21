using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using URFX.Data.DataEntity.DomainModel;
using URFX.Data.Enums;

namespace URFX.Web.Models
{
    public class FinanceBindingModel
    {
        public string Description {get;set;}

        public string Email { get; set; }

        public string ServiceProviderName { get; set; }

        public URFXPaymentType PaymentType { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalEarnedAmount { get; set; }

        public int NumberOfJobs { get; set; }

        public List<JobBindingModel> JobModel { get; set; }

        public string ClientName { get; set; }

        


    }
}