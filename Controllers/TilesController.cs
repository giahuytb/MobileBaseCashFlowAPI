using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly ITileService _tileService;

        public TilesController(ITileService tileService)
        {
            _tileService = tileService;
        }

        [HttpGet("tile")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _tileService.GetAsync();
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

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("tile/{id}")]
        public async Task<ActionResult<Item>> GetById(string id)
        {
            try
            {
                var result = await _tileService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this tile");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("tile")]
        public async Task<ActionResult> PostTile(TileRequest tile)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _tileService.CreateAsync(userId, tile);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("tile/{id}")]
        public async Task<ActionResult> UpdateTile(string id, TileRequest tile)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _tileService.UpdateAsync(id, userId, tile);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("tile/{id}")]
        public async Task<ActionResult> DeleteTile(string id)
        {
            try
            {
                var result = await _tileService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
