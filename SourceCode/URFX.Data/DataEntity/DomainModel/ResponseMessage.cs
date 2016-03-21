using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity.DomainModel
{
    public class ResponseMessage
    {
        public string Message { get; set; }
        public bool ResponseCode { get; set; }
        public object data { get; set; }
        public int totalRecords { get; set; }
    }
}
