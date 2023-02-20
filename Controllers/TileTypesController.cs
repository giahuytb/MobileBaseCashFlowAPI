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

        [HttpGet("board")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _tileTypeService.GetAsync();
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
        [HttpGet("item/{name}")]
        public async Task<ActionResult<Item>> GetById(string id)
        {
            try
            {
                var result = await _tileTypeService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this item");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("item")]
        public async Task<ActionResult> PostBoard(TileTypeRequest tileType)
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
        [HttpPut("board")]
        public async Task<ActionResult> UpdateBoard(string id, TileTypeRequest tileType)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _tileTypeService.UpdateAsync(id, userId, tileType);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("board")]
        public async Task<ActionResult> DeleteBoard(string id)
        {
            try
            {
                var result = await _tileTypeService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
