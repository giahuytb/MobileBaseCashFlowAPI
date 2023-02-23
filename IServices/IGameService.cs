using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(GameRequest gameRequest);
        public Task<string> UpdateAsync(string gameId, GameRequest gameRequest);
        public Task<string> DeleteAsync(string gameId);
    }
}
