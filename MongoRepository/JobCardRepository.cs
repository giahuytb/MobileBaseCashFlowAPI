using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface JobCardRepository
    {
        public Task<IEnumerable<JobCard>> GetAsync();
        public Task<Object?> GetAsync(PaginationFilter filter);
        public Task<object?> GetAsync(string JobCardId);
        public Task<string> CreateAsync(int userId, JobCardRequest request);
        public Task<string> UpdateAsync(string JobCardId, int userId, JobCardRequest request);
        public Task<string> RemoveAsync(string JobCardId);
    }
}
