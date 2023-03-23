using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IJobCardService
    {
        public Task<List<JobCard>> GetAsync();
        public Task<Object?> GetAsync(int pageIndex, int pageSize);
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(JobCardRequest request);
        public Task<string> UpdateAsync(string id, JobCardRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
