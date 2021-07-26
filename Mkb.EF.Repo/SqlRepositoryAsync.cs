using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mkb.EF.Repo
{
    public class SqlRepositoryAsync<TContext> : SqlRepoBase where TContext : DbContext
    {
        public SqlRepositoryAsync(TContext dbSet) : base(dbSet)
        {
        }

        public virtual Task<T> GetFirst<T>(Expression<Func<T, bool>> whereClause) where T : class
        {
            return GetAll(whereClause).FirstOrDefaultAsync();
        }

        public virtual Task AddMany<T>(IEnumerable<T> entity) where T : class
        {
            return _dbSet.Set<T>().AddRangeAsync(entity);
        }


        public virtual async Task AddAndSave<T>(T entity) where T : class
        {
            Add(entity);
            await Save();
        }


        public virtual async Task DeleteAndSave<T>(T entity) where T : class
        {
            Delete(entity);
            await Save();
        }

        public virtual async Task Save()
        {
            await _dbSet.SaveChangesAsync();
        }
    }
}