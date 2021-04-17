using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Mkb.EF.Repo
{
    public abstract class SqlRepoBase
    {
        protected readonly DbContext _dbSet;

        public SqlRepoBase(DbContext dbSet)
        {
            _dbSet = dbSet;
        }

        public virtual void Add<T>(T entity) where T : class
        {
            _dbSet.Set<T>().Add(entity);
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            _dbSet.Set<T>().Remove(entity);
        }

        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            return GetAll<T>(f => true);
        }

        public virtual IQueryable<T> GetAll<T>(Expression<Func<T, bool>> whereClause) where T : class
        {
            return GetAll(whereClause, f => f);
        }

        public virtual IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> whereClause,
            Expression<Func<T, Out>> projection)
            where T : class
        {
            return GetAll(whereClause, projection, true);
        }

        public virtual IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> whereClause,
            Expression<Func<T, Out>> projection,
            bool tracking, params Expression<Func<T, object>>[] includes) where T : class
        {
            var queryB = _dbSet.Set<T>().Where(whereClause);

            if (includes.Any())
            {
                queryB = includes.Aggregate(queryB, (current, item) => current.Include(item));
            }

            if (tracking == false)
            {
                queryB = queryB.AsNoTracking();
            }

            return queryB.Select(projection);
        }
    }
}