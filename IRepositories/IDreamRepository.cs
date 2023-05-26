
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IDreamRepository
    {
        public Task<IEnumerable<Dream>> GetAllAsync();
        public Task<Dream?> GetByIdAsync(string dreamId);
        public Task<IEnumerable<Dream>> GetDreamByModId(int modId);
        public Task<string> CreateAsync(DreamRequest request);
        public Task<string> UpdateAsync(string dreamId, DreamRequest request);
        public Task<string> InActiveAsync(string dreamId);
        public Task<string> RemoveAsync(string dreamId);
    }
}
