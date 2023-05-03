using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface IAssetTypeRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int assetTypeId);
        public Task<string> CreateAsync(int userId, AssetTypeRequest request);
        public Task<string> UpdateAsync(int assetTypeId, int userId, AssetTypeRequest request);
        public Task<string> DeleteAsync(int assetTypeId);
    }
}
