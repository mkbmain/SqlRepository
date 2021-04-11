using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SqlRepository
{
    public interface IRepositoryBase<TbaseDbEntity>
    {
        IQueryable<T> GetAll<T>() where T : TbaseDbEntity;

        IQueryable<T> GetAll<T>(Expression<Func<T, bool>> query) where T : TbaseDbEntity;

        IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> query, Expression<Func<T, Out>> projection, int? skip = null, int? take = null)
            where T : TbaseDbEntity;

        IQueryable<Out> GetAll<T, Out>(Expression<Func<T, bool>> query,
            Expression<Func<T, Out>> projection, bool tracking,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes) where T : TbaseDbEntity;

        Task<T> GetFirst<T>(Expression<Func<T, bool>> query) where T : TbaseDbEntity;
        Task AddMany<T>(IEnumerable<T> entity) where T : TbaseDbEntity;
        Task Add<T>(T entity) where T : TbaseDbEntity;
        Task Delete<T>(T item) where T : TbaseDbEntity;
        Task Update<T>(T entity) where T : TbaseDbEntity;
        Task Save();
    }
}