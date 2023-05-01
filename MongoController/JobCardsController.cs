
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardsController : ControllerBase
    {
        private readonly JobCardRepository _jobCardService;

        public JobCardsController(JobCardRepository jobCardService)
        {
            _jobCardService = jobCardService ?? throw new ArgumentNullException(nameof(jobCardService));
        }

        [HttpGet("all")]
        public async Task<ActionResult<JobCard>> GetAll()
        {
            var result = await _jobCardService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet]
        public async Task<ActionResult<List<JobCard>>> GetByPaging([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _jobCardService.GetAsync(validFilter);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<JobCard>> GetById(string id)
        {
            var jobCard = await _jobCardService.GetAsync(id);
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
        }

        [HttpPost]
        public async Task<ActionResult> PostJobCard(JobCardRequest request)
        {
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _jobCardService.CreateAsync(Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest("Create job card failed");
        }


        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateJobCard(string id, JobCardRequest request)
        {
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _jobCardService.UpdateAsync(id, Int32.Parse(userId), request);
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
