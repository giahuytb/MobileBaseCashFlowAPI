using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameAccountTypeService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, GameAccountTypeRequest gameAccountType);
        public Task<string> UpdateAsync(string accountTypeId, string userId, GameAccountTypeRequest gameAccountType);
        public Task<string> DeleteAsync(string accountTypeId);
    }
}
