
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameRoomsController : ControllerBase
    {
        private readonly IGameRoomRepository _gameRoomRepository;

        public GameRoomsController(IGameRoomRepository gameService)
        {
            _gameRoomRepository = gameService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all game room")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameRoomRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game by game room id")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameRoomRepository.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this game room");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new game room")]
        public async Task<ActionResult> PostGame(GameRoomRequest request)
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
            var result = await _gameRoomRepository.CreateAsync(Int32.Parse(userId), request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing game room")]
        public async Task<ActionResult> UpdateGame(int id, GameRoomRequest request)
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
            var result = await _gameRoomRepository.UpdateAsync(id, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game room")]
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
