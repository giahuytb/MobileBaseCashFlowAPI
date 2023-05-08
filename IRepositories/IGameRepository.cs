using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int gameId);
        public Task<string> CreateAsync(int userId, GameRequest request);
        public Task<string> UpdateAsync(int gameId, int userId, GameRequest request);
        public Task<string> DeleteAsync(int gameId);
    }
}
