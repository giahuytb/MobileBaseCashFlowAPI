using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
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

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            var result = await _gameAccountService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet]
        public async Task<ActionResult<List<GameAccount>>> GetByPaging([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _gameAccountService.GetAsync(validFilter);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GameAccount>> GetGameAccountById(string id)
        {
            var result = await _gameAccountService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this game account");
        }

        [HttpPost]
        public async Task<ActionResult> CreateGameAccount(AccountRequest request)
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

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<List<GameAccount>>> UpdateGameAccount(string id, AccountRequest request)
        {
            var result = await _gameAccountService.GetAsync(id);
            if (result is null)
            {
                return NotFound(result);
            }
            await _gameAccountService.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<List<GameAccount>>> DeleteGameAccount(string id)
        {

            var result = await _gameAccountService.GetAsync(id);
            if (result is null)
            {
                return NotFound(result);
            }
            await _gameAccountService.RemoveAsync(id);
            return Ok(result);
        }
    }
}