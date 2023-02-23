
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialAccountsController : ControllerBase
    {
        private readonly IFinancialAccountService _financialAccountService;

        public FinancialAccountsController(IFinancialAccountService financialAccountService)
        {
            _financialAccountService = financialAccountService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("financial-account")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _financialAccountService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet]
        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("financial-account/{searchBy}&{id}")]
        public async Task<ActionResult<FinancialAccount>> GetById(string searchBy, string id)
        {
            try
            {
                var result = await _financialAccountService.GetAsync(searchBy, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find financial account of this " + searchBy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("financial-account")]
        public async Task<ActionResult> PostFinancialAccount(string financialId, string accountId, double value)
        {
            try
            {
                // get the current user logging in system
                var result = await _financialAccountService.CreateAsync(financialId, accountId, value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
