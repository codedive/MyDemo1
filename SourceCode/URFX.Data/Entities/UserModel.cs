using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace URFX.Data.Entities
{
    public class UserModel
    {
        public string UserName { get; set; }
      
        public string Password { get; set; }
       
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }

        public string ZipCode { get; set; }
    }
    
   
}