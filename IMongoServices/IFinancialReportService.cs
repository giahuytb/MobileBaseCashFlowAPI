using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;
using System.Threading.Tasks;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IFinancialReportService
    {
        public Task<IEnumerable> GetAsync();
        public Task<IEnumerable> GetAsync(int pageIndex, int pageSize);
        public Task<FinancialReport?> GetAsync(string userId);
        public Task<string> GenerateAsync(string userId, FinancialRequest request);
        public Task<string> CreateAsync(string id, int childrenAmount, GameAccountRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
