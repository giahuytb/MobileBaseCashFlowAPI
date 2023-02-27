using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;
using System.Data;

namespace MobileBasedCashFlowAPI.MongoController
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
        public async Task<ActionResult<JobCard>> GetAll()
        {
            var jobCard = await _jobCardService.GetAsync();
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("job-card/{id:length(24)}")]
        public async Task<ActionResult<JobCard>> GetById(string id)
        {
            var jobCard = await _jobCardService.GetAsync(id);
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
        }

        [HttpPost("job-card")]
        public async Task<ActionResult> PostJob(JobCard jobCard)
        {
            try
            {
                await _jobCardService.CreateAsync(jobCard);
                return CreatedAtAction(nameof(GetById), new { id = jobCard._id }, jobCard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("job-card/{id}")]
        public async Task<ActionResult> UpdateJob(string id, JobCard jobCard)
        {
            try
            {
                var jobCard1 = await _jobCardService.GetAsync(id);

                if (jobCard1 is null)
                {
                    return NotFound("can not find this job card");
                }
                await _jobCardService.UpdateAsync(id, jobCard);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
