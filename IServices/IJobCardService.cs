using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IJobCardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, JobCardRequest jobCard);
        public Task<string> UpdateAsync(string jobCardId, string userId, JobCardRequest jobCard);
        public Task<string> DeleteAsync(string jobCardId);
    }
}
