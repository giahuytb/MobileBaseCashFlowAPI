using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using System.Collections;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class GameAccountService : IGameAccountService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<GameAccount> _collection;

        public GameAccountService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<GameAccount>("Game_account");
        }

        public async Task<IEnumerable> GetAsync()
        {
            var gameAccounts = await _collection.Find(_ => true).ToListAsync();
            return gameAccounts;
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
            var gameAccount = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            return gameAccount;
        }

        public async Task<string> CreateAsync(AccountRequest request)
        {
            try
            {
                if (request.Game_account_name.Length <= 0)
                {
                    return "You must to enter game account name";
                }
                else if (request.Game_account_type < 0 || request.Game_account_type > 3)
                {
                    return "You must to enter game account type and it must be from 0-3";
                }
                var account = new GameAccount()
                {
                    Game_account_name = request.Game_account_name,
                    Game_account_type_id = request.Game_account_type,
                };
                await _collection.InsertOneAsync(account);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(string id, AccountRequest request)
        {
            try
            {
                var oldGameAccount = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
                if (oldGameAccount != null)
                {
                    if (request.Game_account_name.Length <= 0)
                    {
                        return "You must to enter game account game";
                    }
                    else if (request.Game_account_type < 0 || request.Game_account_type > 3)
                    {
                        return "You must to enter game account type and it must be from 0-3";
                    }
                    oldGameAccount.Game_account_name = request.Game_account_name;
                    oldGameAccount.Game_account_type_id = request.Game_account_type;

                    await _collection.ReplaceOneAsync(x => x.id == id, oldGameAccount);
                    return SUCCESS;
                }
                else
                {
                    return "Can not found this game account";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.id == id);
            return SUCCESS;
        }


    }
}