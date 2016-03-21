using URFX.Data.Infrastructure.Contract;
using System.Data.Entity;
using System.Transactions;
using URFX.Data.DataEntity;

namespace URFX.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionScope _transaction;
        private readonly URFXDbContext _db;
        public UnitOfWork()
        {
            _db = new URFXDbContext();
        }

        public void Dispose()
        {

        }

        public void StartTransaction()
        {
            _transaction = new TransactionScope();
        }

        public void Commit()
        {
            _db.SaveChanges();
            _transaction.Complete();
        }

        public DbContext Db
        {
            get { return _db; }
        }



    }
}
