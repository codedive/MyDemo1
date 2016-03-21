using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class EmployeeModel
    {
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string ProfileImage { get; set; }

        public string NationalIdAndIqamaNumber { get; set; }

       

        public string LicensePlateNumber { get; set; }

        public string QuickBloxID { get; set; }

        public bool Registred { get; set; }

        public string ServiceProvider { get; set; }
    }
}
