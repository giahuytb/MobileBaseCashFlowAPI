using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public UserRolesController(MobileBasedCashFlowGameContext context)
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
            var userRole = await (from role in _context.UserRoles.Where(r => r.RoleId == id)
                                  select new
                                  {
                                      roleName = role.RoleName,
                                  }).FirstOrDefaultAsync();
            if (userRole == null)
            {
                return NotFound("Can not find this role");
            }

            return Ok(userRole);
        }

        [HttpPost("role")]
        public async Task<ActionResult<UserRole>> PostUserRole(string roleName)
        {
            var role = new UserRole
            {
                RoleId = Guid.NewGuid() + "",
                RoleName = roleName,
                CreateAt = DateTime.Now,
            };
            var check = _context.UserRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);
            if (check.Result != null)
            {
                return Conflict("This Role Has Already Existed");
            }
            await _context.UserRoles.AddAsync(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = role.RoleId }, role);
        }

        [HttpDelete("role/{id}")]
        public async Task<ActionResult> DeleteRole(string roleId)
        {
            try
            {
                var role = await _context.UserRoles.FindAsync(roleId);
                if (role != null)
                {
                    _context.UserRoles.Remove(role);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
