using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IDreamService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, DreamRequest dream);
        public Task<string> UpdateAsync(string dreamId, string userId, DreamRequest dream);
        public Task<string> DeleteAsync(string dreamId);
    }
}
