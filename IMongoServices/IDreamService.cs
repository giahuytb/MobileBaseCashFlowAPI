using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IDreamService
    {
        public Task<List<Dream>> GetAsync();
        public Task<Dream?> GetAsync(string id);
        public Task CreateAsync(Dream dream);
        public Task UpdateAsync(string id, Dream dream);
        public Task RemoveAsync(string id);
    }
}
