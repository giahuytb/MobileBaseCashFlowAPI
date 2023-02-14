using MobieBasedCashFlowAPI.MongoModels;

namespace MobieBasedCashFlowAPI.IServices
{
    public interface IFinancialReportService
    {
        public Task<List<FinancialReportMg>> GetAsync();
        public Task<FinancialReportMg?> GetAsync(string userId);
        public Task CreateAsync(FinancialReportMg financialReportMg);
        public Task UpdateAsync(string id, FinancialReportMg financialReportMg);
        public Task RemoveAsync(string id);
    }
}
