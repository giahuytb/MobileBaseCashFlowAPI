using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.MongoController
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialReport>>> GetAll()
        {
            try
            {
                var financial = await _financialReportService.GetAsync();
                if (financial != null)
                {
                    return Ok(financial);
                }
                return NotFound("list was empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialReport>> GetById(string id)
        {
            var financial = await _financialReportService.GetAsync(id);
            if (financial == null)
            {
                return NotFound("can not find this financial report");
            }
            return Ok(financial);
        }

        [HttpPost]
        public async Task<ActionResult> PostFinacialReport(FinancialRequest request)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return Unauthorized("User id not found, please login");
                }
                var result = await _financialReportService.GenerateAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFinacialReport(string id, int childrenAmount, GameAccountRequest request)
        {
            try
            {
                var finanRp = await _financialReportService.GetAsync(id);

                if (finanRp is null)
                {
                    return NotFound("can not find this financial report");
                }
                var result = await _financialReportService.CreateAsync(id, childrenAmount, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> DeleteFinancialReport(string id)
        {
            try
            {
                var finanRp = await _financialReportService.GetAsync(id);

                if (finanRp is null)
                {
                    return NotFound("can not find this financial report");
                }
                var result = await _financialReportService.RemoveAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
