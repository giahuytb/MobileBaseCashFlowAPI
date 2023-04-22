﻿using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.MongoServices;
using Org.BouncyCastle.Asn1.Ocsp;
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

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
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

            var result = await _jobCardService.CreateAsync(request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest("Create job card failed");


        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> UpdateJobCard(string id, JobCardRequest request)
        {
            var result = await _jobCardService.UpdateAsync(id, request);
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
