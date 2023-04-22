using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface GameMatchRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int matchId);
        public Task<string> CreateAsync(int userId, int gameId, GameMatchRequest request);
        public Task<string> UpdateAsync(int matchId, int UserId, GameMatchRequest request);
        public Task<string> DeleteAsync(int matchId);
    }
}
