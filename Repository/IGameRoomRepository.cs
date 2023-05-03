using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface IGameRoomRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int gameId);
        public Task<string> CreateAsync(int userId, GameRoomRequest request);
        public Task<string> UpdateAsync(int gameId, int userId, GameRoomRequest request);
        public Task<string> DeleteAsync(int gameId);
    }
}
