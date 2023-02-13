using MobieBaseCashFlowAPI.MongoModels;

namespace MobileBaseCashFlowGameAPI.IServices
{
    public interface ITileService
    {
        public Task<List<TileMg>> GetAsync();
        public Task<TileMg?> GetAsync(string id);
        public Task CreateAsync(TileMg tile);
        public Task UpdateAsync(string id, TileMg tile);
        public Task DeleteAsync(string id);
    }
}
