
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using MobileBasedCashFlowAPI.MongoServices;

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

        [HttpGet("all")]
        public async Task<ActionResult<List<Dream?>>> getAll()
        {
            var result = await _dreamService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet]
        public async Task<ActionResult<List<Dream>>> GetByPaging([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var result = await _dreamService.GetAsync(validFilter.PageNumber, validFilter.PageSize);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Dream>> GetById(string id)
        {
            var result = await _dreamService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this dream");
        }

        [HttpPost]
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

        [HttpPut("{id:length(24)}")]
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

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<List<Dream>>> DeleteDream(string id)
        {
            try
            {
                var result = await _dreamService.GetAsync(id);
                if (result is null)
                {
                    return NotFound(result);
                }
                await _dreamService.RemoveAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
