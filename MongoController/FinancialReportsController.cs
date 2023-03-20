using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
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


        [HttpGet("financial-report")]
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

        [HttpGet("financial-report/{id}")]
        public async Task<ActionResult<FinancialReport>> GetById(string id)
        {
            var financial = await _financialReportService.GetAsync(id);
            if (financial == null)
            {
                return NotFound("can not find this financial report");
            }
            return Ok(financial);
        }

        [HttpPost("financial-report")]
        public async Task<ActionResult> PostFinacialReport(FinancialRequest request)
        {
            try
            {
                var result = await _financialReportService.GenerateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("financial-report/{id}")]
        public async Task<ActionResult> UpdateFinacialReport(string id, int childrenAmount, GameAccountRequest request)
        {
            try
            {
                var finanRp = await _financialReportService.GetAsync(id);

                if (finanRp is null)
                {
                    return NotFound("can not find this financial report");
                }
                await _financialReportService.CreateAsync(id, childrenAmount, request);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
