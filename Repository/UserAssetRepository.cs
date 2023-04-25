
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface UserAssetRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int assetId);
        public Task<string> CreateAsync(int assetId, int userId);

        public Task<string> UpdateLastUsedAsync(int assetId, int userId);

    }
}
