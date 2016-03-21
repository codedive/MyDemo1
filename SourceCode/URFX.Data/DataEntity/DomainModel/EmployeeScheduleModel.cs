using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;

namespace URFX.Data.DataEntity.DomainModel
{
   public class EmployeeScheduleModel
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string EmployeeId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string Description { get; set; }

        public int JobId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }
    }
}
