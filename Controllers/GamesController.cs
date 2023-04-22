
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
    public class GamesController : ControllerBase
    {
        private readonly GameRepository _gameService;

        public GamesController(GameRepository gameService)
        {
            _gameService = gameService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _gameService.GetAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            try
            {
                var result = await _gameService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this game");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<ActionResult> PostGame(GameRequest game)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return Unauthorized("User id not Found, please login");
                }
                var result = await _gameService.CreateAsync(Int32.Parse(userId), game);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGame(int id, GameRequest game)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return Unauthorized("User id not Found, please login");
                }
                var result = await _gameService.UpdateAsync(id, Int32.Parse(userId), game);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            try
            {
                var result = await _gameService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
