using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryBase;

namespace SqlRepository
{
    public class SqlRepository<EntityBaseModel> : IRepositoryBase<EntityBaseModel> where EntityBaseModel : SqlDbEntity
    {
        private readonly DbContext _dbSet;

        public SqlRepository(DbContext dbSet)
        {
            _dbSet = dbSet;
        }

        public IQueryable<T> GetAll<T>() where T : EntityBaseModel
        {
            return GetAll<T>(f => true);
        }

        public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> query) where T : EntityBaseModel
        {
            return GetAll(query, f => f);
        }

        public IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> query, Expression<Func<T, Out>> projection, int? skip = null, int? take = null)
            where T : EntityBaseModel
        {
            return GetAll<T, Out>(query, projection, true, skip, take);
        }

        public IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> query, Expression<Func<T, Out>> projection, bool tracking, int? skip = null,
            int? take = null, params Expression<Func<T, object>>[] includes) where T : EntityBaseModel
        {
            var queryB = _dbSet.Set<T>().Where(query).Skip(skip ?? 0);

            if (take != null)
            {
                queryB = queryB.Take(take.Value);
            }

            queryB = includes.Aggregate(queryB, (current, item) => current.Include(item));
            if (tracking == false)
            {
                queryB = queryB.AsNoTracking();
            }

            return queryB.Select(projection);
        }

        public Task<T> GetFirst<T>(Expression<Func<T, bool>> query) where T : EntityBaseModel
        {
            return GetAll<T>(query).FirstOrDefaultAsync();
        }

        public Task AddMany<T>(IEnumerable<T> entity) where T : EntityBaseModel
        {
            return _dbSet.Set<T>().AddRangeAsync(entity);
        }

        public async Task Add<T>(T entity) where T : EntityBaseModel
        {
            // there is a add async but suggested to not be used
            //         This method is async only to allow special value generators, such as the one used by
            //         'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
            //         to access the database asynchronously. For all other cases the non async method should be used.
            // ReSharper disable once MethodHasAsyncOverload
            _dbSet.Set<T>().Add(entity);
        }

        public async Task Delete<T>(T item) where T : EntityBaseModel
        {
            _dbSet.Set<T>().Remove(item);
        }

        public async Task Update<T>(T entity) where T : EntityBaseModel
        {
        }

        public async Task Save()
        {
            await _dbSet.SaveChangesAsync();
        }
    }
}