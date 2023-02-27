using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface ITileService
    {
        public Task<List<Tile>> GetAsync();
        public Task<Tile?> GetAsync(string id);
        public Task CreateAsync(Tile tile);
        public Task UpdateAsync(string id, Tile tile);
        public Task DeleteAsync(string id);
    }
}
