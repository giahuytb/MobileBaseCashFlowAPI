using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;

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


        [HttpGet("financial")]
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

        [HttpGet("financial/{id}")]

        public async Task<ActionResult<FinancialReport>> GetById(string id)
        {
            var financial = await _financialReportService.GetAsync(id);
            if (financial == null)
            {
                return NotFound("can not find this financial report");
            }
            return Ok(financial);
        }

        [HttpPost("financial")]
        public async Task<ActionResult> PostFinacialReport(FinancialReport financialReport)
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
        public async Task<ActionResult> UpdateFinacialReport(string id, int childrenAmount, GameAccount gameAccount)
        {
            try
            {
                var finanRp = await _financialReportService.GetAsync(id);

                if (finanRp is null)
                {
                    return NotFound("can not find this financial report");
                }
                await _financialReportService.UpdateAsync(id, childrenAmount, gameAccount);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
