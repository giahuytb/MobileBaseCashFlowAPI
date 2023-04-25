using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface AssetRepository
    {
        public Task<IEnumerable> GetAsync(int userId);
        public Task<object?> GetByIdAsync(int assetId);
        public Task<string> CreateAsync(int userId, AssetRequest request);
        public Task<string> UpdateAsync(int assetId, int userId, AssetRequest request);
        public Task<string> DeleteAsync(int assetId);
    }
}
