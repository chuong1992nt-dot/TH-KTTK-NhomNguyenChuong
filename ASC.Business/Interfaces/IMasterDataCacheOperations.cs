using ASC.Model.Models;
using System.Threading.Tasks;

namespace ASC.Business.Interfaces
{
    public interface IMasterDataCacheOperations
    {
        Task<MasterDataCache> GetMasterDataCacheAsync();
        Task CreateMasterDataCacheAsync();
    }
}