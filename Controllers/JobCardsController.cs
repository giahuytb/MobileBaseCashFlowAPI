using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardsController : ControllerBase
    {
        private readonly IJobCardService _jobCardService;

        public JobCardsController(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
        }

        [HttpGet("job-card")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _jobCardService.GetAsync();
                if (result == null)
                {
                    return NotFound("list is empty");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("job-card/{id}")]
        public async Task<ActionResult<JobCard>> GetById(string id)
        {
            try
            {
                var result = await _jobCardService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this job card");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("job-card")]
        public async Task<ActionResult> PostJobCard(JobCardRequest jobCard)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _jobCardService.CreateAsync(userId, jobCard);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("job-card/{id}")]
        public async Task<ActionResult> UpdateJobCard(string id, JobCardRequest jobCard)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _jobCardService.UpdateAsync(id, userId, jobCard);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("job-card/{id}")]
        public async Task<ActionResult> DeleteTileType(string id)
        {
            try
            {
                var result = await _jobCardService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
