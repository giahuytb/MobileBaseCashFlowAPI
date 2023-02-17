using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IMgJobCardService
    {
        public Task<List<JobCardMg>> GetAsync();
        public Task<JobCardMg?> GetAsync(string id);
        public Task CreateAsync(JobCardMg jobCard);
        public Task UpdateAsync(string id, JobCardMg jobCard);
        public Task RemoveAsync(string id);
    }
}
