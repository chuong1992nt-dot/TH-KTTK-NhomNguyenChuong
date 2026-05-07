using ASC.Business.Interfaces;
using ASC.Model.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ASC.Business
{
    public class MasterDataCacheOperations : IMasterDataCacheOperations
    {
        private readonly IDistributedCache _cache;
        private readonly IMasterDataOperations _masterData;
        private const string CacheKey = "MasterDataCache";

        public MasterDataCacheOperations(IDistributedCache cache, IMasterDataOperations masterData)
        {
            _cache = cache;
            _masterData = masterData;
        }

        public async Task<MasterDataCache> GetMasterDataCacheAsync()
        {
            var cached = await _cache.GetStringAsync(CacheKey);
            if (cached != null)
                return JsonConvert.DeserializeObject<MasterDataCache>(cached);

            await CreateMasterDataCacheAsync();
            return await GetMasterDataCacheAsync();
        }

        public async Task CreateMasterDataCacheAsync()
        {
            var keys = await _masterData.GetMasterDataKeysAsync();
            var cache = new MasterDataCache
            {
                MasterDataKeys = keys,
                MasterDataValues = new List<MasterDataValue>()
            };
            await _cache.SetStringAsync(CacheKey, JsonConvert.SerializeObject(cache));
        }
    }
}