using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IFinancialAccountService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string searchBy, string id);
        public Task<string> CreateAsync(string finacialId, string gameAccountId, double value);
        public Task<string> UpdateAsync(string finacialId, string gameAccountId, double value);

    }
}
