
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IUserAssetRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int userId);
        public Task<string> CreateAsync(int assetId, int userId);
        public Task<string> UpdateLastUsedAsync(int assetId, int userId);

    }
}
