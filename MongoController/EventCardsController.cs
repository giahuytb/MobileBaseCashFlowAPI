using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;

namespace MobileBasedCashFlowAPI.MongoController
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

        [HttpGet("all")]
        public async Task<ActionResult<List<EventCard>>> GetAll()
        {
            var eventCard = await _eventCardService.GetAsync();
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("list is empty");
        }

        [HttpGet]
        public async Task<ActionResult<List<EventCard>>> Get([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var result = await _eventCardService.GetAsync(validFilter.PageNumber, validFilter.PageSize);
            if (result != null)
            {
                //return Ok(new
                //{
                //    pageIndex = validFilter.PageNumber,
                //    pageSize = validFilter.PageSize,
                //    data = eventCard
                //});
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<EventCard>> GetById(string id)
        {
            var eventCard = await _eventCardService.GetAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find this event card");
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent(EventCardRequest request)
        {
            try
            {
                var result = await _eventCardService.CreateAsync(request);
                if (result == "success")
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateEvent(string id, EventCardRequest request)
        {
            try
            {
                var eventCard = await _eventCardService.GetAsync(id);
                if (eventCard is null)
                {
                    return NotFound("can not find this event card");
                }
                var result = await _eventCardService.UpdateAsync(id, request);
                if (result == "success")
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            try
            {
                var eventCard = await _eventCardService.GetAsync(id);
                if (eventCard is null)
                {
                    return NotFound("can not find this event card");
                }
                var result = await _eventCardService.RemoveAsync(id);
                if (result == "success")
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }


    }
}
