using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IFinancialReportService
    {
        public Task<List<FinancialReport>> GetAsync();
        public Task<FinancialReport?> GetAsync(string userId);
        public Task<string> GenerateAsync(FinancialRequest request);
        public Task<string> CreateAsync(string id, int childrenAmount, GameAccountRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
