using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IMongoRepositories;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using System.Collections;
using X.PagedList;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.MongoController;
using MobileBasedCashFlowAPI.Cache;

namespace MobileBasedCashFlowAPI.MongoRepositories
{
    public class EventCardRepository : IEventCardRepository
    {
        private readonly IMongoCollection<EventCard> _collection;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EventCardsController> _logger;

        public EventCardRepository(MongoDbSettings settings, ILogger<EventCardsController> logger, IMemoryCache cache)
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
                _logger.Log(LogLevel.Information, "Event Card list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Event Card list not found in cache. Fetching from database.");
                eventCardList = await _collection.Find(evt => evt.Status.Equals(true)).ToListAsync();
                _cache.Set(CacheKeys.EventCards, eventCardList, CacheEntryOption.MemoryCacheEntryOption());
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

        public async Task<IEnumerable> GetByTypeIdAsync(int typeId)
        {
            var eventCards = await _collection.Find(evt => evt.Event_type_id == typeId && evt.Status.Equals(true)).ToListAsync();
            return eventCards;
        }

        public async Task<IEnumerable<EventCard>> GetByModIdAsync(int modId)
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of event card from cache.");
            if (_cache.TryGetValue(CacheKeys.EventCards + modId, out IEnumerable<EventCard> eventCardList))
            {
                _logger.Log(LogLevel.Information, "Event Card list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Event Card list not found in cache. Fetching from database.");
                eventCardList = await _collection.Find(evt => evt.Status.Equals(true) && evt.Game_mode_id.Equals(modId)).ToListAsync();
                _cache.Set(CacheKeys.EventCards + modId, eventCardList, CacheEntryOption.MemoryCacheEntryOption());
            }
            return eventCardList;
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
                Game_mode_id = request.Game_mod_id,
                Status = true,
            };
            await _collection.InsertOneAsync(eventCard);

            var eventCardListInMemory = _cache.Get(CacheKeys.EventCards + eventCard.Game_mode_id) as List<EventCard>;
            // check if the cache have value or not
            if (eventCardListInMemory != null)
            {
                // add new object for this list
                eventCardListInMemory.Add(eventCard);
                // remove all value from this cache key
                _cache.Remove(CacheKeys.EventCards + eventCard.Game_mode_id);
                // set new list for this cache by using the list above
                _cache.Set(CacheKeys.EventCards + eventCard.Game_mode_id, eventCardListInMemory);
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
                //oldEventCard.Game_mode_id = request.Game_mod_id;
                oldEventCard.Action = request.Action;

                await _collection.ReplaceOneAsync(x => x.id == id, oldEventCard);

                var eventCardListInMemory = _cache.Get(CacheKeys.EventCards + oldEventCard.Game_mode_id) as List<EventCard>;
                // check if the cache have value or not
                if (eventCardListInMemory != null)
                {
                    // find object that match the id
                    var oldEventCardInMemory = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                    // find it index for update
                    var oldEventCardInMemoryIndex = eventCardListInMemory.FindIndex(x => x.id == id);
                    // check if it exist or not
                    if (oldEventCardInMemory != null)
                    {
                        // remove old object from this list
                        eventCardListInMemory.Remove(oldEventCardInMemory);
                        // insert to list based on index and new object 
                        eventCardListInMemory.Insert(oldEventCardInMemoryIndex, oldEventCard);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.EventCards + oldEventCard.Game_mode_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.EventCards + oldEventCard.Game_mode_id, eventCardListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> InActiveAsync(string id)
        {
            var EventCard = await _collection.Find(evt => evt.id == id).FirstOrDefaultAsync();
            if (EventCard != null)
            {
                if (!EventCard.Status)
                {
                    return "This event card is already inactive";
                }
                EventCard.Status = false;
                await _collection.ReplaceOneAsync(x => x.id == id, EventCard);

                var eventCardListInMemory = _cache.Get(CacheKeys.EventCards + EventCard.Game_mode_id) as List<EventCard>;
                // check if the cache have value or not
                if (eventCardListInMemory != null)
                {
                    // Find event card to delete in cache memory by id
                    var EventCardToDelete = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (EventCardToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the event card we choice
                        eventCardListInMemory.Remove(EventCardToDelete);
                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.EventCards + EventCard.Game_mode_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.EventCards + EventCard.Game_mode_id, eventCardListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> RemoveAsync(string id)
        {
            // Find this event card by id
            var eventCardExist = await _collection.Find(evt => evt.id == id).FirstOrDefaultAsync();
            if (eventCardExist != null)
            {
                var result = await _collection.DeleteOneAsync(x => x.id == id);

                var eventCardListInMemory = _cache.Get(CacheKeys.EventCards + eventCardExist.Game_mode_id) as List<EventCard>;
                // check if the cache have value or not
                if (eventCardListInMemory != null)
                {
                    // Find event card to delete in cache memory by id
                    var EventCardToDelete = eventCardListInMemory.FirstOrDefault(x => x.id == id);
                    // check if it exist or not 
                    if (EventCardToDelete != null)
                    {
                        // Remove old cache and set new cache that deleted the event card we choice
                        eventCardListInMemory.Remove(EventCardToDelete);

                        // remove all value from this cache key
                        _cache.Remove(CacheKeys.EventCards + eventCardExist.Game_mode_id);
                        // set new list for this cache by using the list above
                        _cache.Set(CacheKeys.EventCards + eventCardExist.Game_mode_id, eventCardListInMemory);
                    }
                }
                return Constant.Success;
            }
            return Constant.NotFound;
        }



    }
}
