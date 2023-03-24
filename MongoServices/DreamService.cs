using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections;
using X.PagedList;

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

        public async Task<IEnumerable> GetAsync()
        {
            var dream = await _collection.Find(_ => true).ToListAsync();
            return dream;
        }

        public async Task<object?> GetAsync(PaginationFilter filter, double? from, double? to)
        {
            // var AllDream = await _collection.Find(_ => true).ToListAsync();
            var AllDream = _collection.AsQueryable();
            #region Filter
            if (from.HasValue)
            {
                AllDream = AllDream.Where(d => d.Cost >= from);
            }
            if (to.HasValue)
            {
                AllDream = AllDream.Where(d => d.Cost <= to);
            }

            #endregion

            #region Paging
            var PagedData = await AllDream.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            #endregion


            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
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
                var dream1 = new Dream()
                {
                    Name = request.Name,
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

                    oldDream.Name = request.Name;
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
