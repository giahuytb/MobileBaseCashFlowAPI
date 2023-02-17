using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IItemService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string name);
        public Task<string> CreateAsync(string userId, ItemRequest item);
        public Task<string> UpdateAsync(string itemId, string userId, ItemRequest item);
        public Task<string> DeleteAsync(string itemId);
    }
}
