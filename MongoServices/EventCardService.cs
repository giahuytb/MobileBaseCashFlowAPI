using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using System.Collections;
using X.PagedList;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.MongoController;
using MobileBasedCashFlowAPI.Cache;

namespace MobileBasedCashFlowAPI.MongoServices
{
    public class EventCardService : IEventCardService
    {
        private readonly IMongoCollection<EventCard> _collection;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EventCardsController> _logger;

        public EventCardService(MongoDbSettings settings, ILogger<EventCardsController> logger, IMemoryCache cache)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<EventCard>("Event_card");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<EventCard>> GetAsync()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of event card from cache.");
            if (_cache.TryGetValue(CacheKeys.EventCards, out IEnumerable<EventCard> eventCardList))
            {
                _logger.Log(LogLevel.Information, "Employee list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Event Card list not found in cache. Fetching from database.");
                eventCardList = await _collection.Find(evt => evt.Status.Equals(true)).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(5))
                .SetAbsoluteExpiration(TimeSpan.FromHours(5))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);
                _cache.Set(CacheKeys.EventCards, eventCardList, cacheEntryOptions);
            }
            return eventCardList;
        }

        public async Task<object?> GetAsync(PaginationFilter filter, double? from, double? to)
        {
            var AllEventCard = _collection.AsQueryable();
            #region Filter
            if (from.HasValue)
            {
                AllEventCard = AllEventCard.Where(evt => evt.Cost >= from);
            }
            if (to.HasValue)
            {
                AllEventCard = AllEventCard.Where(evt => evt.Cost <= to);
            }
            #endregion

            #region Paging
            var PagedData = await AllEventCard.ToPagedListAsync(filter.PageIndex, filter.PageSize);
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

        public async Task<IEnumerable> GetAsync(int typeId)
        {
            var eventCards = await _collection.Find(evt => evt.Event_type_id == typeId && evt.Status.Equals(true)).ToListAsync();
            return eventCards;
        }

        public async Task<EventCard?> GetAsync(string id)
        {
            var eventCard = await _collection.Find(evt => evt.id == id && evt.Status.Equals(true)).FirstOrDefaultAsync();
            return eventCard;
        }

        public async Task<string> CreateAsync(EventCardRequest request)
        {
            var eventCard = new EventCard()
            {
                Event_name = request.Event_name,
                Image_url = request.Image_url,
                Account_name = request.Account_Name,
                Event_description = request.Event_description,
                Trading_range = request.Trading_range,
                Cost = request.Cost,
                Down_pay = request.Down_pay,
                Dept = request.Dept,
                Cash_flow = request.Cash_flow,
                Event_type_id = request.Event_type_id,
                Action = request.Action,
                Status = true,
            };
            await _collection.InsertOneAsync(eventCard);

            var eventCardListInMemory = _cache.Get(CacheKeys.EventCards) as List<EventCard>;
            if (eventCardListInMemory != null)
            {
                eventCardListInMemory.Add(eventCard);
                _cache.Remove(CacheKeys.EventCards);
                _cache.Set(CacheKeys.EventCards, eventCardListInMemory);
            }
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(string id, EventCardRequest request)
        {
            var oldEventCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldEventCard != null)
            {
                oldEventCard.Event_name = request.Event_name;
                oldEventCard.Image_url = request.Image_url;
                oldEventCard.Account_name = request.Account_Name;
                oldEventCard.Event_description = request.Event_description;
                oldEventCard.Trading_range = request.Trading_range;
                oldEventCard.Cost = request.Cost;
                oldEventCard.Down_pay = request.Down_pay;
                oldEventCard.Dept = request.Dept;
                oldEventCard.Cash_flow = request.Cash_flow;
                oldEventCard.Event_type_id = request.Event_type_id;
                oldEventCard.Action = request.Action;

                await _collection.ReplaceOneAsync(x => x.id == id, oldEventCard);

                var eventCardListInMemory = _cache.Get(CacheKeys.EventCards) as List<EventCard>;
                if (eventCardListInMemory != null)
                {
                    var oldEventCardInMemory = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                    var oldEventCardInMemoryIndex = eventCardListInMemory.FindIndex(x => x.id == id);
                    if (oldEventCardInMemory != null)
                    {
                        eventCardListInMemory.Remove(oldEventCardInMemory);
                        eventCardListInMemory.Insert(oldEventCardInMemoryIndex, oldEventCard);

                        _cache.Remove(CacheKeys.EventCards);
                        _cache.Set(CacheKeys.EventCards, eventCardListInMemory);
                    }

                }
                return Constant.Success;
            }
            return Constant.NotFound;

        }

        public async Task<string> InActiveCardAsync(string id)
        {
            var oldEventCard = await _collection.Find(account => account.id == id).FirstOrDefaultAsync();
            if (oldEventCard != null)
            {
                oldEventCard.Status = false;
                await _collection.ReplaceOneAsync(x => x.id == id, oldEventCard);

                var eventCardListInMemory = _cache.Get(CacheKeys.EventCards) as List<EventCard>;
                if (eventCardListInMemory != null)
                {
                    var EventCardToDelete = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                    if (EventCardToDelete != null)
                    {
                        eventCardListInMemory.Remove(EventCardToDelete);
                        _cache.Remove(CacheKeys.EventCards);
                        _cache.Set(CacheKeys.EventCards, eventCardListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;

        }

        public async Task<string> RemoveAsync(string id)
        {
            // Find this event card by id
            var eventCardExist = GetAsync(id);
            if (eventCardExist != null)
            {
                // Check if delete action is success or not
                var result = await _collection.DeleteOneAsync(x => x.id == id);

                if (result != null)
                {
                    // Check if the event card list is in memory or not
                    var eventCardListInMemory = _cache.Get(CacheKeys.EventCards) as List<EventCard>;
                    if (eventCardListInMemory != null)
                    {
                        // Find event card to delete in cache memory
                        var EventCardToDelete = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                        if (EventCardToDelete != null)
                        {
                            // Remove old cache and set new cache that deleted the event card we choice
                            eventCardListInMemory.Remove(EventCardToDelete);
                            _cache.Remove(CacheKeys.EventCards);
                            _cache.Set(CacheKeys.EventCards, eventCardListInMemory);
                        }
                    }
                    return Constant.Success;
                }
            }
            return Constant.NotFound;
        }


    }
}
