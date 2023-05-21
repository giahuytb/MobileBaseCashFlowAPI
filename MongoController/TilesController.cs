
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.IMongoRepositories;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TilesController : ControllerBase
    {
        private readonly ITileRepository _tileService;

        public TilesController(ITileRepository tileService)
        {
            _tileService = tileService;
        }

        [HttpGet("all")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<ActionResult<List<Tile>>> GetAll()
        {
            var tile = await _tileService.GetAsync();
            if (tile != null)
            {
                return Ok(tile);
            }
            return NotFound("list is empty");
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Tile>> GetById(string id)
        {
            var tile = await _tileService.GetAsync(id);
            if (tile != null)
            {
                return Ok(tile);
            }
            return NotFound("can not find this tile");
        }

        [HttpPost]
        public async Task<IActionResult> PostTile(Tile tile)
        {
            await _tileService.CreateAsync(tile);
            return CreatedAtAction(nameof(GetById), new { id = tile.id }, tile);

        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateTile(string id, Tile tile)
        {
            var Tile = await _tileService.GetAsync(id);
            if (Tile is null)
            {
                return NotFound("can not find this tile");
            }
            await _tileService.UpdateAsync(id, tile);
            return Ok("update success");
        }



    }
}
