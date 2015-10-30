using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URFX.Data.Infrastructure.Contract
{
    /// <summary>
    /// Marks a service that can be used to access a database
    /// </summary>
    /// <typeparam name="T">The domain object return type</typeparam>
    public interface IDbRepository<T> : IBaseRepository<T>
    {

    }
}
