using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFinancialReportService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string id);
        public Task<string> CreateAsync(string userId, FinancialReportRequest financialReport);
        public Task<string> UpdateAsync(string financialReportId, string userId, FinancialReportRequest financialReport);
        public Task<string> DeleteAsync(string financialReportId);
    }
}
