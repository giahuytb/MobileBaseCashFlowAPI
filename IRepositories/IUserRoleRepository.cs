
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IUserRoleRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<object?> GetByIdAsync(int roleId);
        public Task<string> CreateAsync(string roleName);
        public Task<string> UpdateAsync(int roleId, string roleName);
        public Task<string> DeleteAsync(int roleId);
    }
}
