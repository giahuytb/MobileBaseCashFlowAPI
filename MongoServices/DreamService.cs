
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class DreamService : DreamRepository
    {
        private readonly IMongoCollection<Dream> _collection;
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _cache;

        public DreamService(MongoDbSettings settings, IMemoryCache cache, IDistributedCache distributedCache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Dream>("Dream");

            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task<IEnumerable<Dream>> GetAsync()
        {
            //if (!_cache.TryGetValue(CacheKeys.Dreams, out IEnumerable<Dream> dreamList))
            //{
            //    dreamList = await _collection.Find(_ => true).ToListAsync();
            //    _cache.Set(CacheKeys.Dreams, dreamList, CacheEntryOption.MemoryCacheEntryOption());
            //}
            //return dreamList;

            string? cacheMember = await _distributedCache.GetStringAsync(CacheKeys.Dreams);

            IEnumerable<Dream> dream;
            if (string.IsNullOrEmpty(cacheMember))
            {
                dream = await _collection.Find(_ => true).ToListAsync();
                if (dream is null)
                {
                    return dream ?? Enumerable.Empty<Dream>();
                }

                await _distributedCache.SetStringAsync(
                    CacheKeys.Dreams,
                    JsonConvert.SerializeObject(dream));
                return dream;
            }
            dream = JsonConvert.DeserializeObject<IEnumerable<Dream>>(
                cacheMember,
                new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                }) ?? Enumerable.Empty<Dream>();
            return dream ?? Enumerable.Empty<Dream>();
        }

        public async Task<object?> GetAsync(PaginationFilter filter, double? from, double? to)
        {
            // var AllDream = await _collection.Find(_ => true).ToListAsync();
            var AllDream = _collection.AsQueryable();
            #region Filter
            if (from.HasValue)
            {
                AllDream = AllDream.Where(d => d.Cost >= from);
            }
            if (to.HasValue)
            {
                AllDream = AllDream.Where(d => d.Cost <= to);
            }
            #endregion

            #region Paging
            var PagedData = await AllDream.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            #endregion

            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<Dream?> GetAsync(string id)
        {
            var dream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
            return dream;
        }

        public async Task<string> CreateAsync(DreamRequest request)
        {
            var checkDreamExist = await _collection.Find(dream => dream.Name == request.Name).FirstOrDefaultAsync();
            if (checkDreamExist != null)
            {
                return "This dream name has already existed";
            }
            var dream = new Dream()
            {
                Name = request.Name,
                Cost = request.Cost,
                Status = true,
            };
            await _collection.InsertOneAsync(dream);

            var dreamListInMemory = _cache.Get(CacheKeys.Dreams) as List<Dream>;
            // check if the cache have value or not
            if (dreamListInMemory != null)
            {
                // add new object for this list
                dreamListInMemory.Add(dream);
                // remove all value from this cache key
                _cache.Remove(CacheKeys.Dreams);
                // set new list for this cache by using the list above
                _cache.Set(CacheKeys.Dreams, dreamListInMemory);
            }
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, DreamRequest request)
        {
            var oldDream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
            if (oldDream != null)
            {
                oldDream.Name = request.Name;
                oldDream.Cost = request.Cost;

                await _collection.ReplaceOneAsync(x => x.id == id, oldDream);

                var dreamListInMemory = _cache.Get(CacheKeys.Dreams) as List<Dream>;
                // check if the cache have value or not
                if (dreamListInMemory != null)
                {
                    // find object that match the id
                    var oldDreamInMemory = dreamListInMemory.FirstOrDefault(x => x.id == id);
                    // find it index for update
                    var oldDreamInMemoryIndex = dreamListInMemory.FindIndex(x => x.id == id);
                    // check if it exist or not
                    if (oldDreamInMemory != null)
                    {
                        // remove old object from this list
                        dreamListInMemory.Remove(oldDreamInMemory);
                        // insert to list based on index and new object 
                        dreamListInMemory.Insert(oldDreamInMemoryIndex, oldDream);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.Dreams);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams, dreamListInMemory);
                        return Constant.Success;
                    }
                    return Constant.Failed;
                }
            }
            return Constant.NotFound;
        }

        public async Task<string> InActiveAsync(string id)
        {
            var Dream = await _collection.Find(evt => evt.id == id).FirstOrDefaultAsync();
            if (Dream != null)
            {
                if (!Dream.Status)
                {
                    return "This dream is already inactive";
                }
                Dream.Status = false;
                await _collection.ReplaceOneAsync(x => x.id == id, Dream);

                var DreamListInMemory = _cache.Get(CacheKeys.Dreams) as List<Dream>;
                // check if the cache have value or not
                if (DreamListInMemory != null)
                {
                    // Find event card to delete in cache memory by id
                    var DreamToDelete = DreamListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (DreamToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the event card we choice
                        DreamListInMemory.Remove(DreamToDelete);
                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.Dreams);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams, DreamListInMemory);

                        return Constant.Success;
                    }
                    return Constant.Failed;
                }
            }
            return Constant.NotFound;
        }

        public async Task<string> RemoveAsync(string id)
        {
            var dreamExist = await GetAsync(id);
            if (dreamExist != null)
            {
                await _collection.DeleteOneAsync(x => x.id == id);

                var dreamListInMemory = _cache.Get(CacheKeys.Dreams) as List<Dream>;
                // check if the cache have value or not
                if (dreamListInMemory != null)
                {
                    // Find dream to delete in cache memory by id
                    var dreamToDelete = dreamListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (dreamToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the dream we choice
                        dreamListInMemory.Remove(dreamToDelete);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.Dreams);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams, dreamListInMemory);

                        return Constant.Success;
                    }
                    return Constant.Failed;
                }
            }
            return Constant.NotFound;
        }


    }
}
