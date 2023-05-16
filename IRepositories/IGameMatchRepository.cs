using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameMatchRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<int> TotalMatchInDay();
        public Task<int> TotalMatchInWeek();
        public Task<object?> GetAsync(string matchId);
        public Task<string> CreateAsync(int userId, GameMatchRequest request);
        public Task<string> UpdateAsync(string matchId, GameMatchRequest request);
        public Task<string> DeleteAsync(string matchId);
    }
}
