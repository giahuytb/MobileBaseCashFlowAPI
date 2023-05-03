using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections;
using System.Security.Claims;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleRepository _userRoleServicecs;

        public UserRolesController(IUserRoleRepository userRoleServicecs)
        {
            _userRoleServicecs = userRoleServicecs;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _userRoleServicecs.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _userRoleServicecs.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpPost]
        public async Task<ActionResult> PostUserRole([FromBody] string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userRoleServicecs.CreateAsync(roleName);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserRole(int id, [FromBody] string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userRoleServicecs.UpdateAsync(id, roleName);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var result = await _userRoleServicecs.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
