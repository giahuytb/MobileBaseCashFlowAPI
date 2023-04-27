using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameMatchesController : ControllerBase
    {
        private readonly GameMatchRepository _gameMatchService;

        public GameMatchesController(GameMatchRepository gameMatchService)
        {
            _gameMatchService = gameMatchService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameMatchService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GameMatch>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameMatchService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this board");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<ActionResult> PostMatch(int gameId, GameMatchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameMatchService.CreateAsync(Int32.Parse(userId), gameId, request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMatch(int id, GameMatchRequest request)
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
            var result = await _gameMatchService.UpdateAsync(id, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameMatchService.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

    }
}
