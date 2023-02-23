using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IJobAccountService
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string searchBy, string id);
        public Task<string> CreateAsync(string jobCardId, string gameAccountId, double value);
    }
}
