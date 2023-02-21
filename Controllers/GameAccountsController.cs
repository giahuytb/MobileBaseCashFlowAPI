
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
    public class GameAccountsController : ControllerBase
    {
        private readonly IGameAccountService _gameAccountService;

        public GameAccountsController(IGameAccountService gameAccountService)
        {
            _gameAccountService = gameAccountService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("game-account")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _gameAccountService.GetAsync();
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
        [HttpGet("game-account/{id}")]
        public async Task<ActionResult<GameAccount>> GetById(string id)
        {
            try
            {
                var result = await _gameAccountService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this game account");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("game-account")]
        public async Task<ActionResult> PostGameAccount(GameAccountRequest gameAccount)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameAccountService.CreateAsync(userId, gameAccount);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("game-account/{id}")]
        public async Task<ActionResult> UpdateGameAccount(string id, GameAccountRequest gameAccount)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameAccountService.UpdateAsync(id, userId, gameAccount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("game-account/{id}")]
        public async Task<ActionResult> DeleteGameAccount(string id)
        {
            try
            {
                var result = await _gameAccountService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
