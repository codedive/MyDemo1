﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URFX.Data.Entities;
using URFX.Data.Infrastructure;
using URFX.Data.Infrastructure.Contract;

namespace URFX.Data
{
   public class ClientRatingRepository: BaseRepository<ClientRating>
    {
        public ClientRatingRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
