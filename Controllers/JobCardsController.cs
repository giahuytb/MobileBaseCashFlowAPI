
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.MongoModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardsController : ControllerBase
    {
        private readonly IJobCardRepository _jobCardService;

        public JobCardsController(IJobCardRepository jobCardService)
        {
            _jobCardService = jobCardService ?? throw new ArgumentNullException(nameof(jobCardService));
        }

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all job card")]
        public async Task<ActionResult<JobCard>> GetAll()
        {
            var result = await _jobCardService.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:length(24)}")]
        [SwaggerOperation(Summary = "Get list job card by job card id")]
        public async Task<ActionResult<JobCard>> GetById(string id)
        {
            var jobCard = await _jobCardService.GetByIdAsync(id);
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new job card")]
        public async Task<ActionResult> PostJobCard(JobCardRequest request)
        {
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _jobCardService.CreateAsync(int.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest("Create job card failed");
        }


        [HttpPut("{id:length(24)}")]
        [SwaggerOperation(Summary = "Update an existing job card")]
        public async Task<ActionResult> UpdateJobCard(string id, JobCardRequest request)
        {
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _jobCardService.UpdateAsync(id, int.Parse(userId), request);
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

        [HttpPut("inactive/{id:length(24)}")]
        [SwaggerOperation(Summary = "Inactive an existing job card")]
        public async Task<ActionResult> InActiveJobCard(string id)
        {
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _jobCardService.InActiveAsync(id);
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
        [SwaggerOperation(Summary = "Delete an job card")]
        public async Task<ActionResult> DeleteJobCard(string id)
        {
            var result = await _jobCardService.RemoveAsync(id);
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

    }
}
