
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IRepositories;
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
        private readonly IGameServerRepository _gameRepository;

        public GameServersController(IGameServerRepository gameServerRepository)
        {
            _gameRepository = gameServerRepository;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all game")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game server by game id")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameRepository.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this game");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new game")]
        public async Task<ActionResult> PostGameServer(GameServerRequest request)
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
            var result = await _gameRepository.CreateAsync(Int32.Parse(userId), request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing game")]
        public async Task<ActionResult> UpdateGameServer(int id, GameServerRequest request)
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
            var result = await _gameRepository.UpdateAsync(id, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game")]
        public async Task<ActionResult> DeleteGameServer(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }
}
