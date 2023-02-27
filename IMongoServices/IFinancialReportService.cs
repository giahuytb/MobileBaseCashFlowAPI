using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IFinancialReportService
    {
        public Task<List<FinancialReport>> GetAsync();
        public Task<List<FinancialReport>> GetFinancialAccount(string userId);
        public Task<FinancialReport?> GetAsync(string userId);
        public Task<string> CreateAsync(FinancialReport financialReport);
        public Task<string> UpdateAsync(string id, int childrenAmount, GameAccount gameAccount);
        public Task<string> RemoveAsync(string id);
    }
}
