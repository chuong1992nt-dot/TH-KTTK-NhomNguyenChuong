using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> FindAsync(params object[] keyValues); 
        Task<IEnumerable<T>> GetAllAsync(); 
        Task<IEnumerable<T>> FindAllByAsync(Expression<Func<T, bool>> predicate);
    }
}