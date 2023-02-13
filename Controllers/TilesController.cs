using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobieBaseCashFlowAPI.MongoModels;
using MobileBaseCashFlowGameAPI.IServices;

namespace MobileBaseCashFlowGameAPI.Controllers
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<List<TileMg>>> GetAll()
        {
            var tile = await _tileService.GetAsync();
            if (tile != null)
            {
                return Ok(new { StatusCode = 200, Message = "Request was successfully", data = tile });
            }
            return NotFound(new { StatusCode = 404, Message = "List was empty" });
        }

        [HttpGet("tile/{id:length(24)}")]
        public async Task<ActionResult<TileMg>> GetById(string id)
        {
            var tile = await _tileService.GetAsync(id);
            if (tile != null)
            {
                return Ok(new { StatusCode = 200, Message = "Request was successfully", data = tile });
            }
            return NotFound(new { StatusCode = 404, Message = "Can not found this tile" });
        }

        [HttpPost("tile")]
        public async Task<IActionResult> PostTile(TileMg tile)
        {
            try
            {
                await _tileService.CreateAsync(tile);
                return CreatedAtAction(nameof(GetById), new { id = tile._id }, tile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
            }
        }

        [HttpPut("tile/{id:length(24)}")]
        public async Task<IActionResult> UpdateTile(string id, TileMg tile)
        {
            try
            {
                var Tile = await _tileService.GetAsync(id);
                if (Tile is null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Can not found this tile" });
                }
                await _tileService.UpdateAsync(id, tile);
                return Ok(new { StatusCode = 200, Message = "Update tile successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
            }
        }

    }
}
