
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoRepositories
{
    public interface IDreamRepository
    {
        public Task<IEnumerable<Dream>> GetAsync();
        public Task<Dream?> GetAsync(string dreamId);
        public Task<IEnumerable<Dream>> GetDreamByGameModId(int modId);
        public Task<string> CreateAsync(DreamRequest request);
        public Task<string> UpdateAsync(string dreamId, DreamRequest request);
        public Task<string> InActiveAsync(string dreamId);
        public Task<string> RemoveAsync(string dreamId);
    }
}
