using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Web.Models
{
    public class RefundModel
    {
        public string access_code { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string merchant_reference { get; set; }
        public string language { get; set; }
        public string merchant_identifier { get; set; }
        public string signature { get; set; }
        public string command { get; set; }
        public string fort_id { get; set; }

    }
}