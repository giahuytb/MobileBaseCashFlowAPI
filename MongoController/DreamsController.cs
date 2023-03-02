
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DreamsController : ControllerBase
    {
        private readonly IDreamService _dreamService;
        public DreamsController(IDreamService dreamService)
        {
            _dreamService = dreamService;
        }

        [HttpGet("dream")]
        public async Task<ActionResult<List<Dream?>>> getAll()
        {
            var result = await _dreamService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("dream/{id}")]
        public async Task<ActionResult<Dream>> GetById(string id)
        {
            var result = await _dreamService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this dream");
        }

        [HttpPost("dream")]
        public async Task<ActionResult> CreateDream(DreamRequest request)
        {
            try
            {
                var result = await _dreamService.CreateAsync(request);
                if (result == "success")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("dream/{id}")]
        public async Task<ActionResult<List<Dream>>> UpdateDream(string id, DreamRequest request)
        {
            try
            {
                var result = await _dreamService.GetAsync(id);
                if (result is null)
                {
                    return NotFound(result);
                }
                await _dreamService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



    }
}
