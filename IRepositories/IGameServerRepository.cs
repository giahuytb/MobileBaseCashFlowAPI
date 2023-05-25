using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameServerRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int gameServerId);
        public Task<string> CreateAsync(GameServerRequest request);
        public Task<string> UpdateAsync(int gameServerId, GameServerRequest request);
        public Task<string> DeleteAsync(int gameServerId);
    }
}
