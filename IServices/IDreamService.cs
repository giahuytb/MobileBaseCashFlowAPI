using MobieBaseCashFlowAPI.MongoModels;

namespace MobieBaseCashFlowAPI.IServices
{
    public interface IDreamService
    {
        public Task<List<DreamMg>> GetAsync();
        public Task<DreamMg?> GetAsync(string id);
        public Task CreateAsync(DreamMg dream);
        public Task UpdateAsync(string id, DreamMg dream);
        public Task RemoveAsync(string id);
    }
}
