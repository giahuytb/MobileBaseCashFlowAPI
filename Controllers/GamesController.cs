
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _gameRoomRepository;

        public GamesController(IGameRepository gameService)
        {
            _gameRoomRepository = gameService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all game")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameRoomRepository.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game by game id")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameRoomRepository.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this game");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new game")]
        public async Task<ActionResult> PostGame(GameRequest request)
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
            var result = await _gameRoomRepository.CreateAsync(int.Parse(userId), request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing game")]
        public async Task<ActionResult> UpdateGame(int id, GameRequest request)
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
            var result = await _gameRoomRepository.UpdateAsync(id, int.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameRoomRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }

}
