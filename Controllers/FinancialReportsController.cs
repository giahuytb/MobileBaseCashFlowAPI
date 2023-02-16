using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobieBasedCashFlowAPI.IServices;
using MobieBasedCashFlowAPI.MongoModels;

namespace MobieBasedCashFlowAPI.Controllers
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


        [HttpGet("financial")]
        public async Task<ActionResult<IEnumerable<FinancialReportMg>>> GetAll()
        {
            var financial = await _financialReportService.GetAsync();
            if (financial != null)
            {
                return Ok(financial);
            }
            return NotFound("list was empty");
        }

        [HttpGet("financial/{id}")]

        public async Task<ActionResult<FinancialReportMg>> GetById(string userId)
        {
            var financial = await _financialReportService.GetAsync(userId);
            if (financial == null)
            {
                return NotFound("can not find this financial report");
            }
            return Ok(financial);
        }

        [HttpPost("financial")]
        public async Task<ActionResult> PostFinacialReport(FinancialReportMg financialReport)
        {
            try
            {
                await _financialReportService.CreateAsync(financialReport);
                return CreatedAtAction(nameof(GetById), new { id = financialReport._id }, financialReport);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("financial/{id}")]
        public async Task<ActionResult> UpdateFinacialReport(string id, FinancialReportMg financialReport)
        {
            try
            {
                var finanRp = await _financialReportService.GetAsync(id);

                if (finanRp is null)
                {
                    return NotFound("can not find this financial report");
                }
                await _financialReportService.UpdateAsync(id, financialReport);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
