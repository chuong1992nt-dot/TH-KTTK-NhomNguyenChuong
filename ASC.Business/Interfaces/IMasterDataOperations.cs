using ASC.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASC.Business.Interfaces
{
    public interface IMasterDataOperations
    {
        // Các hàm làm việc với MasterDataKey (Nhóm danh mục)
        Task<List<MasterDataKey>> GetMasterDataKeysAsync();
        Task<bool> InsertMasterDataKeyAsync(MasterDataKey masterDataKey);
        Task<bool> UpdateMasterDataKeyAsync(MasterDataKey masterDataKey);

        // Các hàm làm việc với MasterDataValue (Giá trị danh mục)
        Task<List<MasterDataValue>> GetMasterDataValuesAsync(string partitionKey);
        Task<MasterDataValue> GetMasterDataValueAsync(string partitionKey, string rowKey);
        Task<bool> InsertMasterDataValueAsync(MasterDataValue masterDataValue);
        Task<bool> UpdateMasterDataValueAsync(MasterDataValue masterDataValue);
    }
}