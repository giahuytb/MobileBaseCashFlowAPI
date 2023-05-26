using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IJobCardRepository
    {
        public Task<IEnumerable<JobCard>> GetAllAsync();
        public Task<object?> GetByIdAsync(string JobCardId);
        public Task<string> CreateAsync(int userId, JobCardRequest request);
        public Task<string> UpdateAsync(string JobCardId, int userId, JobCardRequest request);
        public Task<string> InActiveAsync(string id);
        public Task<string> RemoveAsync(string JobCardId);
    }
}
