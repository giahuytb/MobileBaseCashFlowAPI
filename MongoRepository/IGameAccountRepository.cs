using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IGameAccountRepository
    {
        public Task<IEnumerable<GameAccount>> GetAsync();
        public Task<Object?> GetAsync(PaginationFilter filter);
        public Task<GameAccount?> GetAsync(string id);
        public Task<string> CreateAsync(AccountRequest request);
        public Task<string> UpdateAsync(string id, AccountRequest request);
        public Task<string> InActiveAsync(string id);
        public Task<string> RemoveAsync(string id);
    }
}