using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface ITileTypeService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, TileTypeRequest tileType);
        public Task<string> UpdateAsync(string tileTypeId, string userId, TileTypeRequest tileType);
        public Task<string> DeleteAsync(string tileTypeId);
    }
}
