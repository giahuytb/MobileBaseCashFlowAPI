
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.Common;
using X.PagedList;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class JobCardService : IJobCardService
    {
        public const string SUCCESS = "success";
        private readonly IMongoCollection<JobCard> _collection;

        public JobCardService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<JobCard>("Job_card");
        }

        public async Task<List<JobCard>> GetAsync()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<object?> GetAsync(PaginationFilter filter)
        {
            var AllJobCard = await _collection.Find(_ => true).ToListAsync();
            var PagedData = await AllJobCard.ToPagedListAsync(filter.PageIndex, filter.PageSize);
            var TotalPage = ValidateInput.totaPage(PagedData.TotalItemCount, filter.PageSize);
            return new
            {
                filter.PageIndex,
                filter.PageSize,
                totalPage = TotalPage,
                data = PagedData,
            };
        }

        public async Task<object?> GetAsync(string id)
        {
            var result = await _collection.Find(x => x.id == id).ToListAsync();
            return result;
        }

        public async Task<string> CreateAsync(JobCardRequest request)
        {
            try
            {
                var jobCard = new JobCard()
                {
                    Job_card_name = request.Job_card_name,
                    Children_cost = request.Children_cost,
                    Game_accounts = request.Game_accounts,
                };
                await _collection.InsertOneAsync(jobCard);
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateAsync(string id, JobCardRequest request)
        {
            try
            {
                var oldJobCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
                if (oldJobCard != null)
                {
                    oldJobCard.Job_card_name = request.Job_card_name;
                    oldJobCard.Children_cost = request.Children_cost;

                    await _collection.ReplaceOneAsync(x => x.id == id, oldJobCard);
                    return SUCCESS;
                }
                else
                {
                    return "Can not found this job card";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> RemoveAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.id == id);
            return SUCCESS;
        }


    }
}
