using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity.DomainModel
{
  public  class ClientModel
    {      
     
        public string ClientId { get; set; }    
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public bool IsActive { get; set; }       
        public bool IsDeleted { get; set; }    
        public string NationalIdNumber { get; set; }      
        public int? NationaltIDType { get; set; }   
    }
}
