using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IEventCardService
    {
        public Task<List<EventCard>> GetAsync();
        public Task<EventCard?> GetAsync(string id);
        public Task<string> CreateAsync(EventCardRequest request);
        public Task<string> UpdateAsync(string id, EventCardRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
