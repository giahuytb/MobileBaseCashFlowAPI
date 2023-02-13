using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobieBaseCashFlowAPI.Models;
using MobileBaseCashFlowGameAPI.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MobieBaseCashFlowAPI.Controllers
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
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
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
