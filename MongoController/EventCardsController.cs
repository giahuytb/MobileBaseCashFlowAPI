using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MobileBasedCashFlowAPI.Cache;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IMongoRepositories;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventCardsController : ControllerBase
    {
        private readonly IEventCardRepository _eventCardService;

        public EventCardsController(IEventCardRepository eventCardService)
        {
            _eventCardService = eventCardService ?? throw new ArgumentNullException(nameof(eventCardService));
        }

        [SwaggerOperation(Summary = "Get all event card")]
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
        [SwaggerOperation(Summary = "Get list event card by paging and search")]
        public async Task<ActionResult<List<EventCard>>> GetByPaging([FromQuery] PaginationFilter filter, double? from, double? to)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var validFilter = new PaginationFilter(filter.PageIndex, filter.PageSize);
            var result = await _eventCardService.GetAsync(validFilter, from, to);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("list is empty");
        }

        [HttpGet("type/{id}")]
        [SwaggerOperation(Summary = "Get list event card by event card type id")]
        public async Task<ActionResult<EventCard>> GetByTypeId(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventCard = await _eventCardService.GetByTypeIdAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find event card of this type");
        }

        [HttpGet("mod-id/{id}")]
        [SwaggerOperation(Summary = "Get list event card by mod id")]
        public async Task<ActionResult<EventCard>> GetByModId(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventCard = await _eventCardService.GetByModIdAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find event card of this mod");
        }

        [HttpGet("{id:length(24)}")]
        [SwaggerOperation(Summary = "Get list event card by event card id")]
        public async Task<ActionResult<EventCard>> GetById(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventCard = await _eventCardService.GetAsync(id);
            if (eventCard != null)
            {
                return Ok(eventCard);
            }
            return NotFound("can not find this event card");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new event card")]
        public async Task<IActionResult> PostEvent(EventCardRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _eventCardService.CreateAsync(request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id:length(24)}")]
        [SwaggerOperation(Summary = "Update an existing event card")]
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
        [SwaggerOperation(Summary = "Inactive an existing event card")]
        public async Task<IActionResult> InActiveEventCard(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _eventCardService.InActiveAsync(id);
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


        [HttpDelete("{id:length(24)}")]
        [SwaggerOperation(Summary = "Delete an event card")]
        public async Task<IActionResult> DeleteEvent(string id)
        {

            var result = await _eventCardService.RemoveAsync(id);
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


    }
}
