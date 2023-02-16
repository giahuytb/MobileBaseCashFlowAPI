using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobieBasedCashFlowAPI.IServices;
using MobieBasedCashFlowAPI.MongoModels;
using MobileBaseCashFlowGameAPI.Common;

namespace MobieBasedCashFlowAPI.Controllers
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
        public async Task<ActionResult<List<DreamMg?>>> getAll()
        {
            var dream = await _dreamService.GetAsync();
            if (dream != null)
            {
                return Ok(dream);
            }
            return NotFound("list is empty");
        }

        [HttpGet("dream/{id}")]
        public async Task<ActionResult<List<DreamMg>>> GetById(string id)
        {
            var dream = await _dreamService.GetAsync(id);
            if (dream != null)
            {
                return Ok(dream);
            }
            return NotFound("can not find this dream");
        }

        [HttpPost("dream")]
        public async Task<ActionResult<List<DreamMg>>> CreateDream(DreamMg dream)
        {
            try
            {
                await _dreamService.CreateAsync(dream);
                return CreatedAtAction(nameof(GetById), new { Id = dream.id }, dream);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("dream/{id}")]
        public async Task<ActionResult<List<DreamMg>>> UpdateDream(string id, DreamMg dream)
        {
            try
            {
                var dream1 = await _dreamService.GetAsync(id);
                if (dream1 is null)
                {
                    return NotFound("can not find this dream");
                }
                await _dreamService.UpdateAsync(id, dream);
                return Ok("update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
