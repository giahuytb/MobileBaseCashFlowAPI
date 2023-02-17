using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class MgDreamService : IMgDreamService
    {
        private readonly IMongoCollection<DreamMg> _dream;

        public MgDreamService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _dream = database.GetCollection<DreamMg>("Dream");
        }

        public async Task<List<DreamMg>> GetAsync()
        {
            return await _dream.Find(_ => true).ToListAsync();
        }

        public async Task<DreamMg?> GetAsync(string id)
        {
            return await _dream.Find(dream => dream.id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(DreamMg dream)
        {
            await _dream.InsertOneAsync(dream);
        }

        public async Task RemoveAsync(string id)
        {
            await _dream.DeleteOneAsync(x => x.id == id);
        }

        public async Task UpdateAsync(string id, DreamMg dream)
        {
            await _dream.ReplaceOneAsync(x => x.id == id, dream);
        }
    }
}
