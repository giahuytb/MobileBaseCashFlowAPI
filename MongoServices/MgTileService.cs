using MongoDB.Driver;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class MgTileService : IMgTileService
    {
        private readonly IMongoCollection<TileMg> _tiles;
        public MgTileService(MongoDbSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            _tiles = database.GetCollection<TileMg>("Tile");
        }
        public async Task<List<TileMg>> GetAsync()
        {
            return await _tiles.Find(_ => true).ToListAsync();
        }

        public async Task<TileMg?> GetAsync(string id)
        {
            return await _tiles.Find(x => x._id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(TileMg newTile)
        {
            await _tiles.InsertOneAsync(newTile);
        }

        public async Task UpdateAsync(string id, TileMg updatedTile)
        {
            await _tiles.ReplaceOneAsync(x => x._id == id, updatedTile);
        }

        public async Task DeleteAsync(string id)
        {
            await _tiles.DeleteOneAsync(x => x._id == id);
        }

    }
}
