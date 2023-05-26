using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IEventCardRepository
    {
        public Task<IEnumerable<EventCard>> GetAllAsync();
        public Task<IEnumerable<EventCard>> GetByModIdAsync(int modId);
        public Task<EventCard?> GetByIdAsync(string id);
        public Task<string> CreateAsync(EventCardRequest request);
        public Task<string> UpdateAsync(string id, EventCardRequest request);
        public Task<string> InActiveAsync(string id);
        public Task<string> RemoveAsync(string id);
    }
}
