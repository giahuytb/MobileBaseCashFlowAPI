using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IPOIRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetByIdAsync(int poiId);
        public Task<string> CreateAsync(int userId, POIRequest request);
        public Task<string> UpdateAsync(int poiId, int userId, POIRequest request);
        public Task<string> DeleteAsync(int poiId);
    }
}
