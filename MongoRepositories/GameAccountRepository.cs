using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoRepositories;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoRepositories
{
    public class GameAccountRepository : IGameAccountRepository
    {
        private readonly IMongoCollection<GameAccount> _collection;
        private readonly IMemoryCache _cache;

        public GameAccountRepository(MongoDbSettings settings, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<GameAccount>("Game_account");
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<GameAccount>> GetAsync()
        {
            // when call this method for the first time then using cache to store the list object
            if (!_cache.TryGetValue(CacheKeys.GameAccounts, out IEnumerable<GameAccount> gameAccountList))
            {
                gameAccountList = await _collection.Find(account => account.Status.Equals(true)).ToListAsync();

                _cache.Set(CacheKeys.GameAccounts, gameAccountList, CacheEntryOption.MemoryCacheEntryOption());
            }
            return gameAccountList;
        }

        public async Task<object?> GetAsync(PaginationFilter filter)
        {
            var AllGameAccount = await _collection.Find(_ => true).ToListAsync();
            var PagedData = await AllGameAccount.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<GameAccount?> GetAsync(string id)
        {
            var gameAccount = await _collection.Find(account => account.id == id && account.Status.Equals(true)).FirstOrDefaultAsync();
            return gameAccount;
        }

        public async Task<string> CreateAsync(AccountRequest request)
        {

            var gameAccount = new GameAccount()
            {
                Game_account_name = request.Game_account_name,
                Game_account_type_id = request.Game_account_type,
                Status = true,
            };
            await _collection.InsertOneAsync(gameAccount);

            var gameAccountListInMemory = _cache.Get(CacheKeys.GameAccounts) as List<GameAccount>;
            // check if the cache have value or not
            if (gameAccountListInMemory != null)
            {
                // add new object for this list
                gameAccountListInMemory.Add(gameAccount);
                // remove all value from this cache key
                _cache.Remove(CacheKeys.GameAccounts);
                // set new list for this cache by using the list above
                _cache.Set(CacheKeys.GameAccounts, gameAccountListInMemory);
            }
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, AccountRequest request)
        {

            var oldGameAccount = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldGameAccount != null)
            {
                oldGameAccount.Game_account_name = request.Game_account_name;
                oldGameAccount.Game_account_type_id = request.Game_account_type;

                await _collection.ReplaceOneAsync(x => x.id == id, oldGameAccount);

                var gameAccountListInMemory = _cache.Get(CacheKeys.GameAccounts) as List<GameAccount>;
                // check if the cache have value or not
                if (gameAccountListInMemory != null)
                {
                    // find object that match the id
                    var oldGameAccountInMemory = gameAccountListInMemory.FirstOrDefault(x => x.id == id);
                    // find it index for update
                    var oldGameAccountInMemoryIndex = gameAccountListInMemory.FindIndex(x => x.id == id);
                    // check if it exist or not
                    if (oldGameAccountInMemory != null)
                    {
                        // remove old object from this list
                        gameAccountListInMemory.Remove(oldGameAccountInMemory);
                        // insert to list based on index and new object 
                        gameAccountListInMemory.Insert(oldGameAccountInMemoryIndex, oldGameAccount);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.GameAccounts);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.GameAccounts, gameAccountListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> InActiveAsync(string id)
        {
            var GameAccount = await _collection.Find(evt => evt.id == id).FirstOrDefaultAsync();
            if (GameAccount != null)
            {
                if (!GameAccount.Status)
                {
                    return "This game account is already inactive";
                }
                GameAccount.Status = false;
                await _collection.ReplaceOneAsync(x => x.id == id, GameAccount);

                var GameAccountsListInMemory = _cache.Get(CacheKeys.GameAccounts) as List<GameAccount>;
                // check if the cache have value or not
                if (GameAccountsListInMemory != null)
                {
                    // Find event card to delete in cache memory by id
                    var GameAccountToDelete = GameAccountsListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (GameAccountToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the event card we choice
                        GameAccountsListInMemory.Remove(GameAccountToDelete);
                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.GameAccounts);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.GameAccounts, GameAccountsListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> RemoveAsync(string id)
        {
            var gameAccountExist = GetAsync(id);
            if (gameAccountExist != null)
            {
                await _collection.DeleteOneAsync(x => x.id == id);
                var gameAccountListInMemory = _cache.Get(CacheKeys.GameAccounts) as List<GameAccount>;
                // check if the cache have value or not
                if (gameAccountListInMemory != null)
                {
                    // Find game account to delete in cache memory by id
                    var gameAccountToDelete = gameAccountListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (gameAccountToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the game account we choice
                        gameAccountListInMemory.Remove(gameAccountToDelete);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.GameAccounts);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.GameAccounts, gameAccountListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }


    }
}