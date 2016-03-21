using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace URFX.Data.DataEntity.DomainModel
{
    public class PagingModel
    {
        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public string SearchText { get; set; }
    }
}