using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface ITileRepository
    {
        public Task<List<Tile>> GetAllAsync();
        public Task<Tile?> GetByIdAsync(string id);
        public Task<string> CreateAsync(Tile tile);
        public Task<string> UpdateAsync(string id, Tile tile);
        public Task<string> DeleteAsync(string id);
    }
}
