using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface IGameModRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int gameModeId);
        public Task<string> CreateAsync(int userId, GameModeRequest request);
        public Task<string> UpdateAsync(int gameModeId, int userId, GameModeRequest request);
        public Task<string> DeleteAsync(int gameModeId);
    }
}
