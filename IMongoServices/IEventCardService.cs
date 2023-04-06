using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IEventCardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<Object?> GetAsync(PaginationFilter filter, double? from, double? to);
        public Task<IEnumerable> GetAsync(int typeId);
        public Task<EventCard?> GetAsync(string id);
        public Task<string> CreateAsync(EventCardRequest request);
        public Task<string> UpdateAsync(string id, EventCardRequest request);
        public Task<string> InActiveCardAsync(string id);
        public Task<string> RemoveAsync(string id);
    }
}
