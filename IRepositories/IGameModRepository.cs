using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameModRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int gameModeId);
        public Task<string> CreateAsync(int userId, GameModeRequest request);
        public Task<string> UpdateAsync(int gameModeId, int userId, GameModeRequest request);
        public Task<string> DeleteAsync(int gameModeId);
    }
}
