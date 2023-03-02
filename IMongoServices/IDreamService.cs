
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IDreamService
    {
        public Task<List<Dream>> GetAsync();
        public Task<Dream?> GetAsync(string id);
        public Task<string> CreateAsync(DreamRequest request);
        public Task<string> UpdateAsync(string id, DreamRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
