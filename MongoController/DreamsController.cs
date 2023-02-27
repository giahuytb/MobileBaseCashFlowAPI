
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DreamsController : ControllerBase
    {
        private readonly IDreamService _mgDreamService;
        public DreamsController(IDreamService mgDreamService)
        {
            _mgDreamService = mgDreamService;
        }

        [HttpGet("dream")]
        public async Task<ActionResult<List<Dream?>>> getAll()
        {
            var dream = await _mgDreamService.GetAsync();
            if (dream != null)
            {
                return Ok(dream);
            }
            return NotFound("list is empty");
        }

        [HttpGet("dream/{id}")]
        public async Task<ActionResult<List<Dream>>> GetById(string id)
        {
            var dream = await _mgDreamService.GetAsync(id);
            if (dream != null)
            {
                return Ok(dream);
            }
            return NotFound("can not find this dream");
        }

        [HttpPost("dream")]
        public async Task<ActionResult<List<Dream>>> CreateDream(Dream dream)
        {
            try
            {
                await _mgDreamService.CreateAsync(dream);
                return CreatedAtAction(nameof(GetById), new { Id = dream.id }, dream);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("dream/{id}")]
        public async Task<ActionResult<List<Dream>>> UpdateDream(string id, Dream dream)
        {
            try
            {
                var dream1 = await _mgDreamService.GetAsync(id);
                if (dream1 is null)
                {
                    return NotFound("can not find this dream");
                }
                await _mgDreamService.UpdateAsync(id, dream);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }



    }
}
