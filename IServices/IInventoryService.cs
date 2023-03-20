
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IInventoryService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string ItemId, string userId);

    }
}
