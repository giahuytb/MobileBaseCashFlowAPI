
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.IRepositories;

using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameModsController : ControllerBase
    {
        private readonly IGameModRepository _gameModRepository;

        public GameModsController(IGameModRepository gameModeRepository)
        {
            _gameModRepository = gameModeRepository;
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all game mod")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameModRepository.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game mode by game mod id")]
        public async Task<ActionResult<GameMod>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameModRepository.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new game mod")]
        public async Task<ActionResult> PostGameMode(GameModeRequest request)
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
            var result = await _gameModRepository.CreateAsync(Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update an existing game mod")]
        public async Task<ActionResult> UpdateGameMode(int gameModeId, GameModeRequest request)
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
            var result = await _gameModRepository.UpdateAsync(gameModeId, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game mod")]
        public async Task<ActionResult> DeleteGameMode(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameModRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }


    }
}
