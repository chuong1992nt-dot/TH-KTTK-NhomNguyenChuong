using ASC.Business.Interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASC.Business
{
    public class MasterDataOperations : IMasterDataOperations
    {
        private readonly IUnitOfWork _unitOfWork;

        public MasterDataOperations(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MasterDataKey>> GetMasterDataKeysAsync()
        {
            var repository = _unitOfWork.Repository<MasterDataKey>();
            var keys = await repository.GetAllAsync();
            return keys.ToList();
        }

        public async Task<bool> InsertMasterDataKeyAsync(MasterDataKey masterDataKey)
        {
            try
            {
                var repository = _unitOfWork.Repository<MasterDataKey>();
                await repository.AddAsync(masterDataKey);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMasterDataKeyAsync(MasterDataKey masterDataKey)
        {
            try
            {
                var repository = _unitOfWork.Repository<MasterDataKey>();
                repository.Update(masterDataKey);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<MasterDataValue>> GetMasterDataValuesAsync(string partitionKey)
        {
            var repository = _unitOfWork.Repository<MasterDataValue>();
            var values = await repository.FindAllByAsync(v => v.PartitionKey == partitionKey);
            return values.ToList();
        }

        public async Task<MasterDataValue> GetMasterDataValueAsync(string partitionKey, string rowKey)
        {
            var repository = _unitOfWork.Repository<MasterDataValue>();
 
            var values = await repository.FindAllByAsync(v => v.PartitionKey == partitionKey && v.RowKey == rowKey);
            return values.FirstOrDefault();
        }

        public async Task<bool> InsertMasterDataValueAsync(MasterDataValue masterDataValue)
        {
            try
            {
                var repository = _unitOfWork.Repository<MasterDataValue>();
                await repository.AddAsync(masterDataValue);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMasterDataValueAsync(MasterDataValue masterDataValue)
        {
            try
            {
                var repository = _unitOfWork.Repository<MasterDataValue>();
                repository.Update(masterDataValue);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}