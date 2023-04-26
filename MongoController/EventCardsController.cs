using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCardsController : ControllerBase
    {
        private readonly EventCardRepository _eventCardService;

        public EventCardsController(EventCardRepository eventCardService)
        {
            _eventCardService = eventCardService ?? throw new ArgumentNullException(nameof(eventCardService));
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<EventCard>>> GetAll()
        {
            var result = await _eventCardService.GetAsync();
            if (result != null)
            {
                //throw new Exception("Exception while fetching all the students from the storage.");
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet]
        public async Task<ActionResult<List<EventCard>>> GetByPaging([FromQuery] PaginationFilter filter, double? from, double? to)
        {
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _eventCardService.GetAsync(validFilter, from, to);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("type-id/{id}")]
        public async Task<ActionResult<EventCard>> GetByTypeId(int id)
        {
            var eventCard = await _eventCardService.GetAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find event card of this type");
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

            var result = await _eventCardService.CreateAsync(request);
            if (result == Constant.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateEvent(string id, EventCardRequest request)
        {
            var result = await _eventCardService.UpdateAsync(id, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found this event card");
            }
            return BadRequest(result);
        }

        [HttpPut("inactive/{id:length(24)}")]
        public async Task<IActionResult> InActiveEventCard(string id)
        {
            var result = await _eventCardService.InActiveCardAsync(id);
            if (result == Constant.Success)
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found this event card");
            }
            return BadRequest(result);
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {

            var result = await _eventCardService.RemoveAsync(id);
            if (result == Constant.Success)
            {
                return Ok(result);
            }
            else if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found this event card");
            }
            return BadRequest(result);
        }


    }
}
