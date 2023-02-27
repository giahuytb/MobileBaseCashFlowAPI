using MongoDB.Driver;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class TileService : ITileService
    {
        private readonly IMongoCollection<Tile> _tiles;
        public TileService(MongoDbSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            _tiles = database.GetCollection<Tile>("Tile");
        }
        public async Task<List<Tile>> GetAsync()
        {
            return await _tiles.Find(_ => true).ToListAsync();
        }

        public async Task<Tile?> GetAsync(string id)
        {
            return await _tiles.Find(x => x._id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Tile newTile)
        {
            await _tiles.InsertOneAsync(newTile);
        }

        public async Task UpdateAsync(string id, Tile updatedTile)
        {
            await _tiles.ReplaceOneAsync(x => x._id == id, updatedTile);
        }

        public async Task DeleteAsync(string id)
        {
            await _tiles.DeleteOneAsync(x => x._id == id);
        }

    }
}
