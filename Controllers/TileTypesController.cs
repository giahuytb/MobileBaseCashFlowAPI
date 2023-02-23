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
    public class TileTypesController : ControllerBase
    {
        private readonly ITileTypeService _tileTypeService;

        public TileTypesController(ITileTypeService tileTypeService)
        {
            _tileTypeService = tileTypeService;
        }

        [HttpGet("tile-type")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _tileTypeService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("tile-type/{id}")]
        public async Task<ActionResult<TileType>> GetById(string id)
        {
            try
            {
                var result = await _tileTypeService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this tile type");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("tile-type")]
        public async Task<ActionResult> PostTileType(TileTypeRequest tileType)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _tileTypeService.CreateAsync(userId, tileType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("tile-type/{id}")]
        public async Task<ActionResult> UpdateTileType(string id, TileTypeRequest tileType)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _tileTypeService.UpdateAsync(id, userId, tileType);
                if (result != "success")
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("tile-type/{id}")]
        public async Task<ActionResult> DeleteTileType(string id)
        {
            try
            {
                var result = await _tileTypeService.DeleteAsync(id);
                if (result != "success")
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
