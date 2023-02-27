using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IJobCardService
    {
        public Task<List<JobCard>> GetAsync();
        public Task<JobCard?> GetAsync(string id);
        public Task CreateAsync(JobCard jobCard);
        public Task UpdateAsync(string id, JobCard jobCard);
        public Task RemoveAsync(string id);
    }
}
