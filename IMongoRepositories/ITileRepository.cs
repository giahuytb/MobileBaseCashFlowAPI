using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoRepositories
{
    public interface ITileRepository
    {
        public Task<List<Tile>> GetAsync();
        public Task<Tile?> GetAsync(string id);
        public Task<string> CreateAsync(Tile tile);
        public Task<string> UpdateAsync(string id, Tile tile);
        public Task<string> DeleteAsync(string id);
    }
}
