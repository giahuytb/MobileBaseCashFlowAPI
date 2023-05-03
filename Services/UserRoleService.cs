using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Services
{
    public class UserRoleService : IUserRoleRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;
        public UserRoleService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var result = await (from role in _context.UserRoles
                                select new
                                {
                                    roleId = role.RoleId,
                                    roleName = role.RoleName,
                                }).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<object?> GetAsync(int roleId)
        {
            var result = await (from role in _context.UserRoles
                                where role.RoleId == roleId
                                select new
                                {
                                    roleId = role.RoleId,
                                    roleName = role.RoleName,
                                }).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<string> CreateAsync(string roleName)
        {
            var role = new UserRole
            {
                RoleName = roleName,
                CreateAt = DateTime.Now,
            };
            if (role.RoleName.Length < 1)
            {
                return "You must enter role name";
            }
            var check = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (check != null)
            {
                return "This role has already existed";
            }
            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int roleId, string roleName)
        {
            var oldUserRole = await _context.UserRoles.Where(r => r.RoleId == roleId).FirstOrDefaultAsync();
            if (oldUserRole != null)
            {
                if (roleName == null)
                {
                    return "You must enter role name";
                }
                var check = await _context.UserRoles
                            .Where(r => r.RoleName == roleName && r.RoleName != oldUserRole.RoleName)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
                if (check != null)
                {
                    return "This role has already existed";
                }
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not found this role";
        }

        public async Task<string> DeleteAsync(int roleId)
        {
            var role = await _context.UserRoles.Where(g => g.RoleId == roleId).FirstOrDefaultAsync();
            if (role != null)
            {
                _context.UserRoles.Remove(role);
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not found this role";
        }


    }
}
