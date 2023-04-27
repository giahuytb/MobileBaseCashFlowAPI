using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameModesController : ControllerBase
    {
        private readonly GameModeRepository _gameModeRepository;

        public GameModesController(GameModeRepository gameModeRepository)
        {
            _gameModeRepository = gameModeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameModeRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameMode>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameModeRepository.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpPost]
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
            var result = await _gameModeRepository.CreateAsync(Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPut]
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
            var result = await _gameModeRepository.UpdateAsync(gameModeId, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGameMode(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameModeRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }


    }
}
