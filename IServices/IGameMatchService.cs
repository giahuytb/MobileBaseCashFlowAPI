using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameMatchService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, GameMatchRequest request);
        public Task<string> UpdateAsync(string gameMatchId, string UserId, GameMatchRequest request);
        public Task<string> DeleteAsync(string gameMatchId);
    }
}
