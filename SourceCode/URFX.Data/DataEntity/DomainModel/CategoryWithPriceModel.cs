using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.DataEntity.DomainModel
{
   public class CategoryWithPriceModel
    {
        public string Title { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public int CategoryId { get; set; }

        public string Picture { get; set; }

        public string CategoryDescription { get; set; }
    }
}
