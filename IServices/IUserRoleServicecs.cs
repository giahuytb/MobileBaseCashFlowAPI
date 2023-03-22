
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IUserRoleServicecs
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(string roleId);
        public Task<string> CreateAsync(string roleName);
        public Task<string> UpdateAsync(string roleId, string roleName);
        public Task<string> DeleteAsync(string roleId);
    }
}
