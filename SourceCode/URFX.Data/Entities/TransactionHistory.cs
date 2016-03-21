using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Enums;

namespace URFX.Data.Entities
{
   public class TransactionHistory
    {
        [Key]
        public int CartId { get; set; }

        public int PlanId { get; set; }
        public string UserId { get; set; }

        public int JobId { get; set; }
        public int NumberOfTeams { get; set; }
        public URFXPaymentType URFXPaymentType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Status { get; set; }
        public string AccessCode { get; set; }
        public string ResponseMessage { get; set; }
        public string CardNumber { get; set; }
        public string Eci { get; set; }
        public string FortId { get; set; }
        public string ResponseCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerIp { get; set; }
        public string Currency { get; set; }
        public string Amount { get; set; }
        public string MerchantReference { get; set; }
        public string Command { get; set; }
        public string PaymentOption { get; set; }
        public string Language { get; set; }
        public string ExpiryDate { get; set; }
        public string MerchantIdentifier { get; set; }
        public string Signature { get; set; }

    }
}
