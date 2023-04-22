using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class GameAccountService : IGameAccountService
    {
        private readonly IMongoCollection<GameAccount> _collection;
        private readonly IMemoryCache _cache;

        public GameAccountService(MongoDbSettings settings, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<GameAccount>("Game_account");
            _cache = cache;
        }

        public async Task<IEnumerable<GameAccount>> GetAsync()
        {

            if (!_cache.TryGetValue(CacheKeys.GameAccounts, out IEnumerable<GameAccount> gameAccountList))
            {
                gameAccountList = await _collection.Find(account => account.Status.Equals(true)).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(5))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);

                _cache.Set(CacheKeys.GameAccounts, gameAccountList, cacheEntryOptions);
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

            var account = new GameAccount()
            {
                Game_account_name = request.Game_account_name,
                Game_account_type_id = request.Game_account_type,
            };
            await _collection.InsertOneAsync(account);
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, AccountRequest request)
        {

            var oldGameAccount = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldGameAccount != null)
            {
                oldGameAccount.Game_account_name = request.Game_account_name;
                oldGameAccount.Game_account_type_id = request.Game_account_type;

                var result = await _collection.ReplaceOneAsync(x => x.id == id, oldGameAccount);
                if (result != null)
                {
                    return Constant.Success;
                }
                return "Update this game account failed";
            }
            return Constant.NotFound;
        }

        public async Task<string> RemoveAsync(string id)
        {
            var gameAccountExist = GetAsync(id);
            if (gameAccountExist != null)
            {
                var result = await _collection.DeleteOneAsync(x => x.id == id);
                if (result != null)
                {
                    return Constant.Success;
                }
                return "Delete this game account failed";
            }
            return Constant.NotFound;
        }


    }
}