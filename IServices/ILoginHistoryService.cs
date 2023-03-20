using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface ILoginHistoryService
    {
        public Task<IEnumerable> GetAsync();
        public Task<IEnumerable> GetAsync(string userId);
        public Task<string> WriteLog(string userId);
        public Task<string> UpdateLog(string loginId);
    }
}
