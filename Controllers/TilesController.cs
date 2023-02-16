using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobieBasedCashFlowAPI.MongoModels;
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
                return Ok(tile);
            }
            return NotFound("list is empty");
        }

        [HttpGet("tile/{id:length(24)}")]
        public async Task<ActionResult<TileMg>> GetById(string id)
        {
            var tile = await _tileService.GetAsync(id);
            if (tile != null)
            {
                return Ok(tile);
            }
            return NotFound("can not find this tile");
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
                return BadRequest(ex.ToString());
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
                    return NotFound("can not find this tile");
                }
                await _tileService.UpdateAsync(id, tile);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
