
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class DreamRepository : IDreamRepository
    {
        private readonly IMongoCollection<Dream> _collection;
        private readonly IMemoryCache _cache;

        public DreamRepository(MongoDbSettings settings, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Dream>("Dream");

            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<Dream>> GetAllAsync()
        {
            var dreamList = await _collection.Find(_ => true).ToListAsync();
            return dreamList;
        }

        public async Task<Dream?> GetByIdAsync(string id)
        {
            var dream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
            return dream;
        }

        public async Task<IEnumerable<Dream>> GetDreamByModId(int modId)
        {
            if (!_cache.TryGetValue(CacheKeys.Dreams + modId, out IEnumerable<Dream> dreamList))
            {
                dreamList = await _collection.Find(dream => dream.Game_mod_id == modId).ToListAsync();
                _cache.Set(CacheKeys.Dreams + modId, dreamList, CacheEntryOption.MemoryCacheEntryOption());
            }
            return dreamList;
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
                Game_mod_id = request.Game_mod_id,
            };
            await _collection.InsertOneAsync(dream);

            var dreamListInMemory = _cache.Get(CacheKeys.Dreams + request.Game_mod_id) as List<Dream>;
            // check if the cache have value or not
            if (dreamListInMemory != null)
            {
                // add new object for this list
                dreamListInMemory.Add(dream);
                // remove all value from this cache key
                _cache.Remove(CacheKeys.Dreams + request.Game_mod_id);
                // set new list for this cache by using the list above
                _cache.Set(CacheKeys.Dreams + request.Game_mod_id, dreamListInMemory);
            }
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, DreamRequest request)
        {

            var oldDream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
            if (oldDream != null)
            {
                var checkName = await _collection.Find(dream => dream.Name == request.Name && dream.Name != oldDream.Name)
                                       .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This dream name is existed";
                }

                oldDream.Name = request.Name;
                oldDream.Cost = request.Cost;
                oldDream.Game_mod_id = request.Game_mod_id;

                await _collection.ReplaceOneAsync(x => x.id == id, oldDream);

                var dreamListInMemory = _cache.Get(CacheKeys.Dreams + oldDream.Game_mod_id) as List<Dream>;
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
                        _cache.Remove(CacheKeys.Dreams + oldDream.Game_mod_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams + oldDream.Game_mod_id, dreamListInMemory);

                    }
                }
                return Constant.Success;
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

                var DreamListInMemory = _cache.Get(CacheKeys.Dreams + Dream.Game_mod_id) as List<Dream>;
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
                        _cache.Remove(CacheKeys.Dreams + Dream.Game_mod_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams + Dream.Game_mod_id, DreamListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> RemoveAsync(string id)
        {
            var dreamExist = await GetByIdAsync(id);
            if (dreamExist != null)
            {
                await _collection.DeleteOneAsync(x => x.id == id);

                var dreamListInMemory = _cache.Get(CacheKeys.Dreams + dreamExist.Game_mod_id) as List<Dream>;
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
                        _cache.Remove(CacheKeys.Dreams + dreamExist.Game_mod_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.Dreams + dreamExist.Game_mod_id, dreamListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }


    }
}
