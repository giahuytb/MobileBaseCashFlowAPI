using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IBoardService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, BoardRequest item);
        public Task<string> UpdateAsync(string boardId, string userId, BoardRequest board);
        public Task<string> DeleteAsync(string boardId);
    }
}
