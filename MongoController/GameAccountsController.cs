using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MongoDB.Bson;
using System.Collections;

namespace MobileBasedCashFlowAPI.MongoController
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

        [HttpGet("game-account")]
        public async Task<ActionResult<IEnumerable>> getAll()
        {
            try
            {
                var result = await _gameAccountService.GetAsync();
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

        [HttpGet("game-account/{id}")]
        public async Task<ActionResult<GameAccount>> GetById(string id)
        {
            var result = await _gameAccountService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this game account");
        }

        [HttpPost("game-account")]
        public async Task<ActionResult> CreateGameAccount(AccountRequest request)
        {
            try
            {
                var result = await _gameAccountService.CreateAsync(request);
                if (result == "success")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("game-account/{id}")]
        public async Task<ActionResult<List<GameAccount>>> UpdateGameAccount(string id, AccountRequest request)
        {
            try
            {
                var result = await _gameAccountService.GetAsync(id);
                if (result is null)
                {
                    return NotFound(result);
                }
                await _gameAccountService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
