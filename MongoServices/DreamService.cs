
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class DreamService : IDreamService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<Dream> _collection;

        public DreamService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Dream>("Dream");
        }

        public async Task<List<Dream>> GetAsync()
        {
            var dream = await _collection.Find(_ => true).ToListAsync();
            return dream;
        }

        public async Task<Dream?> GetAsync(string id)
        {
            var dream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
            return dream;
        }

        public async Task<string> CreateAsync(DreamRequest request)
        {
            try
            {
                var checkDreamExist = _collection.Find(dream => dream.Name == request.Name).FirstOrDefaultAsync();
                if (checkDreamExist != null)
                {
                    return "This dream has already existed";
                }
                if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost <= 0)
                {
                    return "Cost must be mumber and bigger than 0";
                }
                if (request.Name.Length <= 0)
                {
                    return "You need to fill name of this dream";
                }
                if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost <= 0)
                {
                    return "You need to fill description of this dream";
                }
                var dream1 = new Dream()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Cost = request.Cost,
                };
                await _collection.InsertOneAsync(dream1);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(string id, DreamRequest request)
        {
            try
            {
                var oldDream = await _collection.Find(dream => dream.id == id).FirstOrDefaultAsync();
                if (oldDream != null)
                {
                    if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost <= 0)
                    {
                        return "Cost must be mumber and bigger than 0";
                    }
                    else if (request.Name.Length <= 0)
                    {
                        return "You need to fill name for this dream";
                    }
                    else if (!ValidateInput.isNumber(request.Cost.ToString()) || request.Cost <= 0)
                    {
                        return "You need to fill description for this dream";
                    }

                    oldDream.Name = request.Name;
                    oldDream.Description = request.Description;
                    oldDream.Cost = request.Cost;

                    var result = await _collection.ReplaceOneAsync(x => x.id == id, oldDream);
                    if (result != null)
                    {
                        return SUCCESS;
                    }
                    return "Update failed";
                }
                else
                {
                    return "Can not found this dream";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoveAsync(string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.id == id);
            if (result != null)
            {
                return SUCCESS;
            }
            return "Can not found this dream to delete";
        }


    }
}
