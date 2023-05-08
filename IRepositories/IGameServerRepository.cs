using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IGameServerRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int gameServerId);
        public Task<string> CreateAsync(int userId, GameServerRequest request);
        public Task<string> UpdateAsync(int gameServerId, int userId, GameServerRequest request);
        public Task<string> DeleteAsync(int gameServerId);
    }
}
