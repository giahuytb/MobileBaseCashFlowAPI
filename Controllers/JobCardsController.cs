﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobieBasedCashFlowAPI.IServices;
using MobieBasedCashFlowAPI.MongoModels;
using System.Data;

namespace MobieBasedCashFlowAPI.Controllers
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
            var jobCard = await _jobCardService.GetAsync();
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("job/{id:length(24)}")]
        public async Task<ActionResult<JobCardMg>> GetById(string id)
        {
            var jobCard = await _jobCardService.GetAsync(id);
            if (jobCard != null)
            {
                return Ok(jobCard);
            }
            return NotFound("can not find this job card");
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
                return BadRequest(ex.ToString());
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
