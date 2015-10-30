using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Text;
using System.Collections.Specialized;
using URFX.Data.Infrastructure.Contract;
using System.Linq.Expressions;
using System.Data.Entity;

namespace URFX.Data.Infrastructure
{
    public class BaseRepository<T> : IBaseRepository<T>
      where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        internal DbSet<T> dbSet;
        public BaseRepository(IUnitOfWork unitOfWork)
        {

            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
            _unitOfWork = unitOfWork;
            this.dbSet = _unitOfWork.Db.Set<T>();
        }


        /// <summary>
        /// Returns the object with the primary key specifies or the default for the type
        /// </summary>
        /// <typeparam name="TU">The type to map the result to</typeparam>
        /// <param name="primaryKey">The primary key</param>
        /// <returns>The result mapped to the specified type</returns>
        public T SingleOrDefault(Expression<Func<T, bool>> whereCondition)
        {
            var dbResult = dbSet.Where(whereCondition).FirstOrDefault();
            return dbResult;
        }

        //public virtual IEnumerable<T> GetAllWithDeleted()
        //{
        //    var dbresult = _unitOfWork.Db.Fetch<T>("");

        //    return dbresult;
        //}

        public bool Exists(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Any(whereCondition);
        }

        public virtual T Insert(T entity)
        {
            dynamic obj = dbSet.Add(entity);
            this._unitOfWork.Db.SaveChanges();
            return obj;
        }


        public virtual void InsertAll(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                dbSet.Add(entity);
                //_unitOfWork.Db.Entry(entity).State = EntityState.Added;
            }
            this._unitOfWork.Db.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            dbSet.Attach(entity);
            _unitOfWork.Db.Entry(entity).State = EntityState.Modified;
            this._unitOfWork.Db.SaveChanges();

        }

        public virtual void UpdateAll(IList<T> entities)
        {
            foreach (var entity in entities)
            {
                dbSet.Attach(entity);
                _unitOfWork.Db.Entry(entity).State = EntityState.Modified;
            }
            this._unitOfWork.Db.SaveChanges();
        }




        public void Delete(Expression<Func<T, bool>> whereCondition)
        {
            IEnumerable<T> entities = this.GetAll(whereCondition);
            foreach (T entity in entities)
            {
                if (_unitOfWork.Db.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
            }
            this._unitOfWork.Db.SaveChanges();
        }

        public IUnitOfWork UnitOfWork { get { return _unitOfWork; } }

        internal DbContext Database { get { return _unitOfWork.Db; } }

        public IEnumerable<T> GetAll()
        {

            return dbSet.AsEnumerable();

        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).AsEnumerable();
        }

        public int Count(Expression<Func<T, bool>> whereCondition)
        {
            return dbSet.Where(whereCondition).Count();
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, Expression<Func<T, string>> thenBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).ThenBy(thenBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, int>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, long>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecords(Expression<Func<T, bool>> whereCondition, Expression<Func<T, DateTime?>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }

        public IEnumerable<T> GetPagedRecordsDecending(Expression<Func<T, bool>> whereCondition, Expression<Func<T, DateTime?>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderByDescending(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }

        public IEnumerable<T> GetPagedRecordsDecending(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderByDescending(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecordsDecending(Expression<Func<T, bool>> whereCondition, Expression<Func<T, string>> orderBy, Expression<Func<T, string>> thenBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderByDescending(orderBy).ThenByDescending(thenBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
        public IEnumerable<T> GetPagedRecordsDecending(Expression<Func<T, bool>> whereCondition, Expression<Func<T, int>> orderBy, int pageNo, int pageSize)
        {
            return (dbSet.Where(whereCondition).OrderByDescending(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize)).AsEnumerable();
        }
       

        public Dictionary<string, string> GetAuditNames(dynamic dynamicObject)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecWithStoreProcedure(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters);
        }
    }
}
