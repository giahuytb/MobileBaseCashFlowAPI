
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Security.Claims;
using System.Collections;
using MobileBasedCashFlowAPI.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameServersController : ControllerBase
    {
        private readonly GameServerRepository _gameServerRepository;

        public GameServersController(GameServerRepository gameServerRepository)
        {
            _gameServerRepository = gameServerRepository;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all game server")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameServerRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game server by game server id")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameServerRepository.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this game");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new game server")]
        public async Task<ActionResult> PostGameServer(GameServerRequest game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameServerRepository.CreateAsync(Int32.Parse(userId), game);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing game server")]
        public async Task<ActionResult> UpdateGameServer(int id, GameServerRequest game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameServerRepository.UpdateAsync(id, Int32.Parse(userId), game);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game server")]
        public async Task<ActionResult> DeleteGameServer(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameServerRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }
}
