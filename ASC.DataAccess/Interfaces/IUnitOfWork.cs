using System;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CommitTransactionAsync();
    }
}