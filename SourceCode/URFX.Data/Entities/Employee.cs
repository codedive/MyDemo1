using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Entities
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Description { get; set; }

        public string  Password { get; set; }

        public string  ConfirmPassword { get; set; }

        public string  ProfileImage { get; set; }

        public string NationalIdAndIqamaNumber { get; set; }

        

        public string LicensePlateNumber { get; set; }

        public string QuickBloxID { get; set; }
        [DefaultValue(false)]
        public bool Registred { get; set; }

      }
}
