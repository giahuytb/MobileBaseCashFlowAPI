using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using Org.BouncyCastle.Utilities;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class JobCardService : IJobCardService
    {
        private IMongoCollection<JobCard> _jobCard;

        public JobCardService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _jobCard = database.GetCollection<JobCard>("Job_card");
        }

        public async Task<List<JobCard>> GetAsync()
        {
            return await _jobCard.Find(_ => true).ToListAsync();
        }

        public async Task<JobCard?> GetAsync(string id)
        {
            return await _jobCard.Find(jc => jc._id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(JobCard jobCard)
        {
            await _jobCard.InsertOneAsync(jobCard);
        }

        public async Task RemoveAsync(string id)
        {
            await _jobCard.DeleteOneAsync(x => x._id == id);
        }

        public async Task UpdateAsync(string id, JobCard jobCard)
        {
            await _jobCard.ReplaceOneAsync(x => x._id == id, jobCard);
        }

    }
}
