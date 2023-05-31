using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameMatchRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<int> TotalMatchInDay();
        public Task<int> TotalMatchInWeek();
        public Task<int> GetTotalUserPlayGameInDay();
        public Task<object?> GetByIdAsync(string matchId);
        public Task<string> CreateAsync(int userId, GameMatchRequest request);
        public Task<string> UpdateAsync(string matchId, GameMatchRequest request);
        public Task<string> DeleteAsync(string matchId);
    }
}
