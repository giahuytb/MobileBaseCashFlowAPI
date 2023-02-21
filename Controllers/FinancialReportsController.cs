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
    public class FinancialReportsController : ControllerBase
    {
        private readonly IFinancialReportService _financialReportService;

        public FinancialReportsController(IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }

        [HttpGet("financial-report")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _financialReportService.GetAsync();
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

        [HttpGet("financial-report/{id}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<FinancialReport>> GetById(string id)
        {
            try
            {
                var result = await _financialReportService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find this financial report");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("financial-report")]
        public async Task<ActionResult> PostEventCard(FinancialReportRequest financialReport)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _financialReportService.CreateAsync(userId, financialReport);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("financial-report/{id}")]
        public async Task<ActionResult> UpdateEventCard(string id, FinancialReportRequest financialReport)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _financialReportService.UpdateAsync(id, userId, financialReport);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("financial-report/{id}")]
        public async Task<ActionResult> DeleteEventCard(string id)
        {
            try
            {
                var result = await _financialReportService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
