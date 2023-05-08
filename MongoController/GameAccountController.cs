using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoRepositories;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameAccountsController : ControllerBase
    {
        private readonly IGameAccountRepository _gameAccountService;
        public GameAccountsController(IGameAccountRepository gameAccountService)
        {
            _gameAccountService = gameAccountService;
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all game account")]
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
        [SwaggerOperation(Summary = "Get list game account by paging")]
        public async Task<ActionResult<List<GameAccount>>> GetByPaging([FromQuery] PaginationFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _gameAccountService.GetAsync(validFilter);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("{id:length(24)}")]
        [SwaggerOperation(Summary = "Get list game account by game account id")]
        public async Task<ActionResult<GameAccount>> GetGameAccountById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameAccountService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this game account");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new game account")]
        public async Task<ActionResult> CreateGameAccount(AccountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameAccountService.CreateAsync(request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id:length(24)}")]
        [SwaggerOperation(Summary = "Update an existing game account")]
        public async Task<ActionResult> UpdateGameAccount(string id, AccountRequest request)
        {
            var result = await _gameAccountService.UpdateAsync(id, request);
            if (result.Equals(Constant.Success))
            {
                return NotFound(result);
            }
            await _gameAccountService.UpdateAsync(id, request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("inactive/{id:length(24)}")]
        [SwaggerOperation(Summary = "Inactive an existing game account")]
        public async Task<ActionResult> InActiveDream(string id)
        {
            // get user id from claim
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameAccountService.InActiveAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id:length(24)}")]
        [SwaggerOperation(Summary = "Delete an game account")]
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