using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;

namespace ASC.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private Dictionary<string, object> repositories;

        // SỬA: Nhận DbContext abstract (đã được đăng ký đúng trong Program.cs)
        public UnitOfWork(DbContext context)
        {
            dbContext = context;
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null)
                repositories = new Dictionary<string, object>();

            var type = typeof(T).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(T)), dbContext);
                repositories.Add(type, repositoryInstance);
            }
            return (IRepository<T>)repositories[type];
        }

        public async Task<int> CommitTransactionAsync()
        {
            // THÊM LOG ĐỂ DEBUG
            Console.WriteLine($">>> DANG LUU VAO DB: {dbContext.GetType().Name}");
            var result = await dbContext.SaveChangesAsync();
            Console.WriteLine($">>> SO DONG DA LUU: {result}");
            return result;
        }

        public void Dispose()
        {
            dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}