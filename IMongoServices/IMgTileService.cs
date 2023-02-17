using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IMgTileService
    {
        public Task<List<TileMg>> GetAsync();
        public Task<TileMg?> GetAsync(string id);
        public Task CreateAsync(TileMg tile);
        public Task UpdateAsync(string id, TileMg tile);
        public Task DeleteAsync(string id);
    }
}
