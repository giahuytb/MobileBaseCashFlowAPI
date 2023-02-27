using Microsoft.AspNetCore.Http;
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
    public class GameMatchsController : ControllerBase
    {
        private readonly IGameMatchService _gameMatchService;

        public GameMatchsController(IGameMatchService gameMatchService)
        {
            _gameMatchService = gameMatchService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("match")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _gameMatchService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("match/{id}")]
        public async Task<ActionResult<GameMatch>> GetById(string id)
        {
            try
            {
                var result = await _gameMatchService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this board");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("match")]
        public async Task<ActionResult> PostMatch(GameMatchRequest request)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameMatchService.CreateAsync(userId, request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("match/{id}")]
        public async Task<ActionResult> UpdateMatch(string id, GameMatchRequest request)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameMatchService.UpdateAsync(id, userId, request);
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

        [HttpDelete("match/{id}")]
        public async Task<ActionResult> DeleteMatch(string id)
        {
            try
            {
                var result = await _gameMatchService.DeleteAsync(id);
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
