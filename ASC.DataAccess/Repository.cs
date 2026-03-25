using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;

namespace ASC.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected DbContext dbContext;
        protected DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAllByAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> FindAsync(params object[] keyValues)
        {
            return await dbSet.FindAsync(keyValues);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}