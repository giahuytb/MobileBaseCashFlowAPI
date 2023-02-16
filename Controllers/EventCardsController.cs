using Microsoft.AspNetCore.Mvc;
using MobileBaseCashFlowGameAPI.IServices;
using MobieBasedCashFlowAPI.MongoModels;

namespace MobileBaseCashFlowGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCardsController : ControllerBase
    {
        private readonly IEventCardService _eventCardService;

        public EventCardsController(IEventCardService eventCardService)
        {
            _eventCardService = eventCardService;
        }

        [HttpGet("event")]
        public async Task<ActionResult<List<EventCardMg>>> GetAll()
        {
            var eventCard = await _eventCardService.GetAsync();
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("list is empty");
        }

        [HttpGet("event/{id:length(24)}")]
        public async Task<ActionResult<EventCardMg>> GetById(string id)
        {
            var eventCard = await _eventCardService.GetAsync(id);

            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find this event card");
        }

        [HttpPost("event")]
        public async Task<IActionResult> PostEvent(EventCardMg eventCard)
        {
            try
            {
                await _eventCardService.CreateAsync(eventCard);
                return CreatedAtAction(nameof(GetById), new { id = eventCard._id }, eventCard);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("event/{id:length(24)}")]
        public async Task<IActionResult> UpdateEvent(string id, EventCardMg updateEventCard)
        {
            try
            {
                var eventCard = await _eventCardService.GetAsync(id);
                if (eventCard is null)
                {
                    return NotFound("can not find this event card");
                }
                await _eventCardService.UpdateAsync(id, updateEventCard);
                return Ok("update success");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
