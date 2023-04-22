
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.Common;
using X.PagedList;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class JobCardService : IJobCardService
    {
        private readonly IMongoCollection<JobCard> _collection;
        private readonly IMemoryCache _cache;

        public JobCardService(MongoDbSettings settings, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<JobCard>("Job_card");
            _cache = cache ?? throw new ArgumentNullException(nameof(cache)); ;
        }

        public async Task<IEnumerable<JobCard>> GetAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.JobCards, out IEnumerable<JobCard> jobcardList))
            {
                jobcardList = await _collection.Find(_ => true).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(5))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);

                _cache.Set(CacheKeys.JobCards, jobcardList, cacheEntryOptions);
            }
            return jobcardList;
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

            var jobCard = new JobCard()
            {
                Job_card_name = request.Job_card_name,
                Children_cost = request.Children_cost,
                Game_accounts = request.Game_accounts,
            };
            await _collection.InsertOneAsync(jobCard);
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, JobCardRequest request)
        {
            var oldJobCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldJobCard != null)
            {
                oldJobCard.Job_card_name = request.Job_card_name;
                oldJobCard.Children_cost = request.Children_cost;

                var result = await _collection.ReplaceOneAsync(x => x.id == id, oldJobCard);
                if (result != null)
                {
                    return Constant.Success;
                }
                return "Update this job card failed";
            }
            return Constant.NotFound;

        }

        public async Task<string> RemoveAsync(string id)
        {
            var jobCardExist = GetAsync(id);
            if (jobCardExist != null)
            {
                var result = await _collection.DeleteOneAsync(x => x.id == id);
                if (result != null)
                {
                    return Constant.Success;
                }
                return "Delete this job card failed";
            }
            return Constant.Success;
        }


    }
}
