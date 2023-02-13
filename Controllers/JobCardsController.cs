using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobieBaseCashFlowAPI.IServices;
using MobieBaseCashFlowAPI.MongoModels;
using System.Data;

namespace MobieBaseCashFlowAPI.Controllers
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

        [HttpGet("job")]
        public async Task<ActionResult<JobCardMg>> GetAll()
        {
            var job = await _jobCardService.GetAsync();
            if (job != null)
            {
                return Ok(new { StatusCode = 200, Message = "Request was successfully", data = job });
            }
            return NotFound(new { StatusCode = 404, Message = "List was empty" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("job/{id:length(24)}")]
        public async Task<ActionResult<JobCardMg>> GetById(string id)
        {
            var job = await _jobCardService.GetAsync(id);
            if (job != null)
            {
                return Ok(new { StatusCode = 200, Message = "Request was successfully", data = job });
            }
            return NotFound(new { StatusCode = 404, Message = "Can not found this job" });
        }

        [HttpPost("job")]
        public async Task<ActionResult> PostJob(JobCardMg jobCard)
        {
            try
            {
                await _jobCardService.CreateAsync(jobCard);
                return CreatedAtAction(nameof(GetById), new { id = jobCard._id }, jobCard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
            }
        }

        [HttpPut("job/{id}")]
        public async Task<ActionResult> UpdateJob(string id, JobCardMg jobCard)
        {
            try
            {
                var jobCard1 = await _jobCardService.GetAsync(id);

                if (jobCard1 is null)
                {
                    return NotFound(new { StatusCode = 404, Message = "Can not found this dream" });
                }
                await _jobCardService.UpdateAsync(id, jobCard);
                return Ok(new { StatusCode = 200, Message = "Update job successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
            }
        }
    }
}
