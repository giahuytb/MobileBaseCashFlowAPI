using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface ILeaderboardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, LeaderboardRequest leaderboard);
        public Task<string> UpdateAsync(string leaderboardId, string userId, LeaderboardRequest leaderboard);
        public Task<string> DeleteAsync(string leaderboardId);
    }
}
