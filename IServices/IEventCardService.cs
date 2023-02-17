using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IEventCardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, EventCardRequest eventCard);
        public Task<string> UpdateAsync(string cardId, string userId, EventCardRequest eventCard);
        public Task<string> DeleteAsync(string cardId);
    }
}
