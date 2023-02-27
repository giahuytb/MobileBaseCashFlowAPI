using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IEventCardService
    {
        public Task<List<EventCard>> GetAsync();
        public Task<EventCard?> GetAsync(string id);
        public Task CreateAsync(EventCard eventCard);
        public Task UpdateAsync(string id, EventCard eventCard);
        public Task RemoveAsync(string id);
    }
}
