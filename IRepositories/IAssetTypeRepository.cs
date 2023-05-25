using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IAssetTypeRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int assetTypeId);
        public Task<string> CreateAsync(AssetTypeRequest request);
        public Task<string> UpdateAsync(int assetTypeId, AssetTypeRequest request);
        public Task<string> DeleteAsync(int assetTypeId);
    }
}
