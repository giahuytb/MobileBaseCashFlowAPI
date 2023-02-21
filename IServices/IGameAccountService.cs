using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameAccountService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, GameAccountRequest gameAccount);
        public Task<string> UpdateAsync(string gameAccountId, string userId, GameAccountRequest gameAccount);
        public Task<string> DeleteAsync(string gameAccountId);
    }
}
