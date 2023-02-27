using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class DreamService : IDreamService
    {
        private readonly IMongoCollection<Dream> _dream;

        public DreamService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _dream = database.GetCollection<Dream>("Dream");
        }

        public async Task<List<Dream>> GetAsync()
        {
            return await _dream.Find(_ => true).ToListAsync();
        }

        public async Task<Dream?> GetAsync(string id)
        {
            return await _dream.Find(dream => dream.id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Dream dream)
        {
            await _dream.InsertOneAsync(dream);
        }

        public async Task RemoveAsync(string id)
        {
            await _dream.DeleteOneAsync(x => x.id == id);
        }

        public async Task UpdateAsync(string id, Dream dream)
        {
            await _dream.ReplaceOneAsync(x => x.id == id, dream);
        }
    }
}
