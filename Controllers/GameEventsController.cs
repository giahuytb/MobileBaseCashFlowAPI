
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameEventsController : ControllerBase
    {
        private readonly IGameEventService _gameEventService;

        public GameEventsController(IGameEventService gameEventService)
        {
            _gameEventService = gameEventService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("game-event")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _gameEventService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("game-event/{id}")]
        public async Task<ActionResult<GameEvent>> GetById(string id)
        {
            try
            {
                var result = await _gameEventService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this game event");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("game-event")]
        public async Task<ActionResult> PostGameEvent(GameEventRequest gameEvent)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameEventService.CreateAsync(userId, gameEvent);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("game-event/{id}")]
        public async Task<ActionResult> UpdateGameEvent(string id, GameEventRequest gameEvent)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameEventService.UpdateAsync(id, userId, gameEvent);
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

        [HttpDelete("game-event/{id}")]
        public async Task<ActionResult> DeleteGameEvent(string id)
        {
            try
            {
                var result = await _gameEventService.DeleteAsync(id);
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
