using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IGameAccountService
    {
        public Task<IEnumerable> GetAsync();
        public Task<GameAccount?> GetAsync(string id);
        public Task<string> CreateAsync(AccountRequest request);
        public Task<string> UpdateAsync(string id, AccountRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
