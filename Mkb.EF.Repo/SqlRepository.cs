using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Mkb.EF.Repo
{
    public class SqlRepository<TContext> : SqlRepoBase where TContext : DbContext
    {
        public SqlRepository(TContext dbSet) : base(dbSet)
        {
        }

        public virtual T GetFirst<T>(Expression<Func<T, bool>> whereClause) where T : class
        {
            return GetAll(whereClause).FirstOrDefault();
        }

        public virtual void AddMany<T>(IEnumerable<T> entity) where T : class
        {
            _dbSet.Set<T>().AddRange(entity);
        }


        public virtual void AddAndSave<T>(T entity) where T : class
        {
            Add(entity);
            Save();
        }


        public virtual void DeleteAndSave<T>(T entity) where T : class
        {
            Delete(entity);
            Save();
        }

        public virtual void Save()
        {
            _dbSet.SaveChanges();
        }
    }
}