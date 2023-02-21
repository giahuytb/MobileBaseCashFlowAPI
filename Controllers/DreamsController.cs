using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DreamsController : ControllerBase
    {
        private readonly IDreamService _dreamService;

        public DreamsController(IDreamService dreamService)
        {
            _dreamService = dreamService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("dream")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _dreamService.GetAsync();
                if (result == null)
                {
                    return NotFound("list is empty");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dream/{id}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<Dream>> GetById(string id)
        {
            try
            {
                var result = await _dreamService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this dream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("dream")]
        public async Task<ActionResult> PostDream(DreamRequest dream)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _dreamService.CreateAsync(userId, dream);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("dream/{id}")]
        public async Task<ActionResult> UpdateDream(string id, DreamRequest dream)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _dreamService.UpdateAsync(id, userId, dream);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("dream/{id}")]
        public async Task<ActionResult> DeleteDream(string id)
        {
            try
            {
                var result = await _dreamService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
