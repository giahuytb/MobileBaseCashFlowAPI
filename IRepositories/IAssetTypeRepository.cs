using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IAssetTypeRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int assetTypeId);
        public Task<string> CreateAsync(AssetTypeRequest request);
        public Task<string> UpdateAsync(int assetTypeId, AssetTypeRequest request);
        public Task<string> DeleteAsync(int assetTypeId);
    }
}
