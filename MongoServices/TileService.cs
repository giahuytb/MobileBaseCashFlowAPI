using MongoDB.Driver;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.Common;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class TileService : ITileService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<Tile> _collection;
        public TileService(MongoDbSettings setting)
        {
            var client = new MongoClient(setting.ConnectionString);
            var database = client.GetDatabase(setting.DatabaseName);
            _collection = database.GetCollection<Tile>("Tile");
        }
        public async Task<List<Tile>> GetAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<object?> GetAsync(PaginationFilter filter)
        {
            var AllTile = await _collection.Find(_ => true).ToListAsync();
            var PagedData = await AllTile.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<Tile?> GetAsync(string id)
        {
            return await _collection.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(Tile newTile)
        {
            await _collection.InsertOneAsync(newTile);
            return SUCCESS;
        }

        public async Task<string> UpdateAsync(string id, Tile updatedTile)
        {
            await _collection.ReplaceOneAsync(x => x.id == id, updatedTile);
            return SUCCESS;
        }

        public async Task<string> DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.id == id);
            return SUCCESS;
        }


    }
}
