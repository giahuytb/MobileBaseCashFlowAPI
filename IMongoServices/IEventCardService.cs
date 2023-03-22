using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IEventCardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<Object?> GetAsync(int pageIndex, int pageSize);
        public Task<EventCard?> GetAsync(string id);
        public Task<string> CreateAsync(EventCardRequest request);
        public Task<string> UpdateAsync(string id, EventCardRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
