using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;
using URFX.Data.Infrastructure;
using URFX.Data.Infrastructure.Contract;

namespace URFX.Data
{
   public class RatingRepository: BaseRepository<Rating>
    {
        public RatingRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
