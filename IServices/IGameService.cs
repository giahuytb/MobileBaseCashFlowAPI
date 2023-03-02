using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(GameRequest request);
        public Task<string> UpdateAsync(string gameId, GameRequest request);
        public Task<string> DeleteAsync(string gameId);
    }
}
