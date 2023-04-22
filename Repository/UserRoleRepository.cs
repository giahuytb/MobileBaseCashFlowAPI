﻿
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface UserRoleRepository
    {
        public Task<IEnumerable> GetAsync();
        public Task<object?> GetAsync(int roleId);
        public Task<string> CreateAsync(string roleName);
        public Task<string> UpdateAsync(int roleId, string roleName);
        public Task<string> DeleteAsync(int roleId);
    }
}
