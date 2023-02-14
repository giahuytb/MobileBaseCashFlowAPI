using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobieBasedCashFlowAPI.Models;
using MobileBaseCashFlowGameAPI.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MobieBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly MobileBaseCashFlowGameContext _context;

        public UserRolesController(MobileBaseCashFlowGameContext context)
        {
            _context = context;
        }

        [HttpGet("role")]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAll()
        {
            return await _context.UserRoles.ToListAsync();
        }

        [HttpGet("role/{id}")]
        public async Task<ActionResult<UserRole>> GetById(string id)
        {
            var userRole = await (from role in _context.UserRoles 
                                  select new
                                  {
                                      roleId = role.RoleId, 
                                      roleName = role.RoleName,
                                  }).FirstOrDefaultAsync();
            if (userRole == null)
            {
                return NotFound();
            }

            return Ok(userRole);
        }

        // POST: api/UserRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("role")]
        public async Task<ActionResult<UserRole>> PostUserRole(string roleName)
        {
            var role = new UserRole
            {
                RoleId = Guid.NewGuid() + "",
                RoleName = roleName,
            };
            var check = _context.UserRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (check.Result != null)
            {
                return Conflict(new { StatusCode = 409, Message = "This Role Has Already Existed" });
            }
            await _context.UserRoles.AddAsync(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, role);
        }
    }
}
