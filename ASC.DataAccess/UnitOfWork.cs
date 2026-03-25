using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;
using ASC.DataAccess;

namespace ASC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private Dictionary<string, object> repositories; // Lưu cache các repository đã gọi

        public UnitOfWork(DbContext context)
        {
            dbContext = context;
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            // Nếu Repository chưa được tạo, thì khởi tạo nó
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), dbContext);
                repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)repositories[type];
        }

        public async Task<int> CommitTransactionAsync()
        {
            // Nơi xử lý lưu thay đổi xuống Database
            return await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}