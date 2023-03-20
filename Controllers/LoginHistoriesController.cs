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
    public class LoginHistoriesController : ControllerBase
    {
        private readonly ILoginHistoryService _loginHistoryService;

        public LoginHistoriesController(ILoginHistoryService loginHistoryService)
        {
            _loginHistoryService = loginHistoryService;
        }

        [HttpGet]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _loginHistoryService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("{userId}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetById(string userId)
        {
            try
            {
                var result = await _loginHistoryService.GetAsync(userId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not found this user's login history");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{loginId}")]
        public async Task<ActionResult> UpdateLoginHistory(string loginId)
        {
            try
            {
                var result = await _loginHistoryService.UpdateLog(loginId);
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
