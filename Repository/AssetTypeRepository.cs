using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface AssetTypeRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int assetTypeId);
        public Task<string> CreateAsync(int userId, AssetTypeRequest request);
        public Task<string> UpdateAsync(int assetTypeId, int userId, AssetTypeRequest request);
        public Task<string> DeleteAsync(int assetTypeId);
    }
}
