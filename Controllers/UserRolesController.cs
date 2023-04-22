using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Services;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly UserRoleRepository _userRoleServicecs;

        public UserRolesController(UserRoleRepository userRoleServicecs)
        {
            _userRoleServicecs = userRoleServicecs;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _userRoleServicecs.GetAsync();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var result = await _userRoleServicecs.GetAsync(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostUserRole(string roleName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userRoleServicecs.CreateAsync(roleName);
                if (result.Equals("success"))
                {
                    return Ok("Create success");
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUserRole(int id, string roleName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _userRoleServicecs.UpdateAsync(id, roleName);
                if (result.Equals("success"))
                {
                    return Ok("Update success");
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _userRoleServicecs.DeleteAsync(id);
                if (role.Equals("success"))
                {
                    Ok("Delete success");
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
