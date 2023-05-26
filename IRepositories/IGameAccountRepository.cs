using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameAccountRepository
    {
        public Task<IEnumerable<GameAccount>> GetAllAsync();
        public Task<GameAccount?> GetByIdAsync(string id);
        public Task<string> CreateAsync(AccountRequest request);
        public Task<string> UpdateAsync(string id, AccountRequest request);
        public Task<string> InActiveAsync(string id);
        public Task<string> RemoveAsync(string id);
    }
}