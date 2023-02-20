using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface ITileService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, TileRequest tile);
        public Task<string> UpdateAsync(string tileId, string userId, TileRequest tile);
        public Task<string> DeleteAsync(string tileId);
    }
}
