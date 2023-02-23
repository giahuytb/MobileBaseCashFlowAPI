
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobAccountsController : ControllerBase
    {
        private readonly IJobAccountService _jobAccountService;

        public JobAccountsController(IJobAccountService jobAccountService)
        {
            _jobAccountService = jobAccountService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("job-account")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _jobAccountService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet]
        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("job-account/{searchBy}&{id}")]
        public async Task<ActionResult<JobAccount>> GetById(string searchBy, string id)
        {
            try
            {
                var result = await _jobAccountService.GetAsync(searchBy, id);
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
        [HttpPost("job-account")]
        public async Task<ActionResult> PostFinancialAccount(string jobCardId, string accountId, double value)
        {
            try
            {
                // get the current user logging in system
                var result = await _jobAccountService.CreateAsync(jobCardId, accountId, value);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
