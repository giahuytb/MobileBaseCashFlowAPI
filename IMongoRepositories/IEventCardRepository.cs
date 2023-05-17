using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoRepositories
{
    public interface IEventCardRepository
    {
        public Task<IEnumerable<EventCard>> GetAsync();
        public Task<object?> GetAsync(PaginationFilter filter, double? from, double? to);
        public Task<IEnumerable<EventCard>> GetByModIdAsync(int modId);
        public Task<IEnumerable> GetByTypeIdAsync(string type);
        public Task<EventCard?> GetAsync(string id);
        public Task<string> CreateAsync(EventCardRequest request);
        public Task<string> UpdateAsync(string id, EventCardRequest request);
        public Task<string> InActiveAsync(string id);
        public Task<string> RemoveAsync(string id);
    }
}
