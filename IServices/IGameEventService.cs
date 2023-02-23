using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameEventService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, GameEventRequest gameEvent);
        public Task<string> UpdateAsync(string eventId, string userId, GameEventRequest gameEvent);
        public Task<string> DeleteAsync(string eventId);
    }
}
