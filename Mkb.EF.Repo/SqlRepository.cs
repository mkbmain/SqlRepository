using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Mkb.EF.Repo
{
    public class SqlRepository : SqlRepoBase
    {
        public SqlRepository(DbContext dbSet) : base(dbSet)
        {
        }

        public T GetFirst<T>(Expression<Func<T, bool>> whereClause) where T : class
        {
            return GetAll(whereClause).FirstOrDefault();
        }

        public void AddMany<T>(IEnumerable<T> entity) where T : class
        {
            _dbSet.Set<T>().AddRange(entity);
        }


        public void AddAndSave<T>(T entity) where T : class
        {
            Add(entity);
            Save();
        }


        public void DeleteAndSave<T>(T entity) where T : class
        {
            Delete(entity);
            Save();
        }

        public void Save()
        {
            _dbSet.SaveChanges();
        }
    }
}