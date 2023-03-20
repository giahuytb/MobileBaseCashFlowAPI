using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IGameReportService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, GameReportRequest request);
        public Task<string> UpdateAsync(string reportId, GameReportRequest request);
        public Task<string> DeleteAsync(string reportId);
    }
}
