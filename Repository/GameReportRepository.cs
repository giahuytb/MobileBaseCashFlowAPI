using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface GameReportRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int reportId);
        public Task<string> CreateAsync(int userId, GameReportRequest request);
        public Task<string> UpdateAsync(int reportId, GameReportRequest request);
        public Task<string> DeleteAsync(int reportId);
    }
}
