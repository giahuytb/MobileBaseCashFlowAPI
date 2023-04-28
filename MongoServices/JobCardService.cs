
using MongoDB.Driver;
using MongoDB.Driver.Linq;

using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.Common;
using X.PagedList;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class JobCardService : JobCardRepository
    {
        private readonly IMongoCollection<JobCard> _collection;
        private readonly IMemoryCache _cache;

        public JobCardService(MongoDbSettings settings, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<JobCard>("Job_card");
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<JobCard>> GetAsync()
        {
            if (!_cache.TryGetValue(CacheKeys.JobCards, out IEnumerable<JobCard> jobcardList))
            {
                jobcardList = await _collection.Find(_ => true).ToListAsync();
                _cache.Set(CacheKeys.JobCards, jobcardList, CacheEntryOption.MemoryCacheEntryOption());
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

        public async Task<string> CreateAsync(int userId, JobCardRequest request)
        {

            var jobCard = new JobCard()
            {
                Job_card_name = request.Job_card_name,
                Children_cost = request.Children_cost,
                Image_url = request.Image_url,
                Game_accounts = request.Game_accounts,
                Status = true,
                Create_at = DateTime.Now,
                Update_at = DateTime.Now,
                Update_by = 0,
                Create_by = userId,

            };
            await _collection.InsertOneAsync(jobCard);

            var jobCardistInMemory = _cache.Get(CacheKeys.JobCards) as List<JobCard>;
            // check if the cache have value or not
            if (jobCardistInMemory != null)
            {
                // add new object for this list
                jobCardistInMemory.Add(jobCard);
                // remove all value from this cache key
                _cache.Remove(CacheKeys.JobCards);
                // set new list for this cache by using the list above
                _cache.Set(CacheKeys.JobCards, jobCardistInMemory);
            }
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, int userId, JobCardRequest request)
        {
            var oldJobCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldJobCard != null)
            {
                oldJobCard.Job_card_name = request.Job_card_name;
                oldJobCard.Children_cost = request.Children_cost;
                oldJobCard.Image_url = request.Image_url;
                oldJobCard.Update_at = DateTime.Now;
                oldJobCard.Update_by = userId;

                await _collection.ReplaceOneAsync(x => x.id == id, oldJobCard);

                var jobCardListInMemory = _cache.Get(CacheKeys.JobCards) as List<JobCard>;
                // check if the cache have value or not
                if (jobCardListInMemory != null)
                {
                    // find object that match the id
                    var oldJobCardInMemory = jobCardListInMemory.FirstOrDefault(x => x.id == id);
                    // find it index for update
                    var oldJobCardInMemoryIndex = jobCardListInMemory.FindIndex(x => x.id == id);
                    // check if it exist or not
                    if (oldJobCardInMemory != null)
                    {
                        // remove old object from this list
                        jobCardListInMemory.Remove(oldJobCardInMemory);
                        // insert to list based on index and new object 
                        jobCardListInMemory.Insert(oldJobCardInMemoryIndex, oldJobCard);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.JobCards);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.JobCards, jobCardListInMemory);
                        return Constant.Success;
                    }
                    return Constant.Failed;
                }
            }
            return Constant.NotFound;

        }

        public async Task<string> RemoveAsync(string id)
        {
            var jobCardExist = GetAsync(id);
            if (jobCardExist != null)
            {
                var result = await _collection.DeleteOneAsync(x => x.id == id);
                var jobCardListInMemory = _cache.Get(CacheKeys.JobCards) as List<Dream>;
                // check if the cache have value or not
                if (jobCardListInMemory != null)
                {
                    // Find job card to delete in cache memory by id
                    var jobCardToDelete = jobCardListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (jobCardToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the job card we choice
                        jobCardListInMemory.Remove(jobCardToDelete);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.JobCards);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.JobCards, jobCardListInMemory);

                        return Constant.Success;
                    }
                    return Constant.Failed;
                }
            }
            return Constant.Success;
        }


    }
}
