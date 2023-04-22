using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Services
{
    public class UserRoleService : UserRoleRepository
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;
        public UserRoleService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            try
            {
                var result = await (from role in _context.UserRoles
                                    select new
                                    {
                                        roleId = role.RoleId,
                                        roleName = role.RoleName,
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<object?> GetAsync(int roleId)
        {
            try
            {
                var result = await (from role in _context.UserRoles
                                    where role.RoleId == roleId
                                    select new
                                    {
                                        roleId = role.RoleId,
                                        roleName = role.RoleName,
                                    }).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> CreateAsync(string roleName)
        {
            try
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
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> UpdateAsync(int roleId, string roleName)
        {
            try
            {
                var oldUserRole = await _context.UserRoles.Where(r => r.RoleId == roleId).FirstOrDefaultAsync();
                if (oldUserRole != null)
                {
                    if (roleName == null)
                    {
                        return "You must enter role name";
                    }
                    var check = await _context.UserRoles.Where(r => r.RoleName == roleName && r.RoleName != oldUserRole.RoleName).FirstOrDefaultAsync();
                    if (check != null)
                    {
                        return "This role has already existed";
                    }
                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                return "Can not found this user role";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteAsync(int roleId)
        {
            var role = await _context.UserRoles.Where(g => g.RoleId == roleId).FirstOrDefaultAsync();
            if (role != null)
            {
                _context.UserRoles.Remove(role);
                return SUCCESS;
            }
            return "Can not found this user role";
        }



    }
}
