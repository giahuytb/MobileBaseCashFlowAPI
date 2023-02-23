
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
    public class GameAccountTypesController : ControllerBase
    {
        private readonly IGameAccountTypeService _gameAccountTypeService;

        public GameAccountTypesController(IGameAccountTypeService gameAccountTypeService)
        {
            _gameAccountTypeService = gameAccountTypeService;
        }

        [HttpGet("account-type")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _gameAccountTypeService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("account-type/{id}")]
        public async Task<ActionResult<GameAccountType>> GetById(string id)
        {
            try
            {
                var result = await _gameAccountTypeService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this account type");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("account-type")]
        public async Task<ActionResult> PostAccountType(GameAccountTypeRequest gameAccountType)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameAccountTypeService.CreateAsync(userId, gameAccountType);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("account-type/{id}")]
        public async Task<ActionResult> UpdateAccountType(string id, GameAccountTypeRequest gameAccountType)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _gameAccountTypeService.UpdateAsync(id, userId, gameAccountType);
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

        [HttpDelete("account-type/{id}")]
        public async Task<ActionResult> DeleteAccountType(string id)
        {
            try
            {
                var result = await _gameAccountTypeService.DeleteAsync(id);
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
