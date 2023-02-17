using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IMgDreamService
    {
        public Task<List<DreamMg>> GetAsync();
        public Task<DreamMg?> GetAsync(string id);
        public Task CreateAsync(DreamMg dream);
        public Task UpdateAsync(string id, DreamMg dream);
        public Task RemoveAsync(string id);
    }
}
