using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
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

        [HttpGet("event-card")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _eventCardService.GetAsync();
                if (result == null)
                {
                    return NotFound("List is empty");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("event-card/{id}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<EventCard>> GetById(string id)
        {
            try
            {
                var result = await _eventCardService.GetAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not found this event card");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("event-card")]
        public async Task<ActionResult> PostEventCard(EventCardRequest eventCard)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login again ");
                }
                var result = await _eventCardService.CreateAsync(userId, eventCard);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("event-card/{id}")]
        public async Task<ActionResult> UpdateEventCard(string id, EventCardRequest eventCard)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login again");
                }
                var result = await _eventCardService.UpdateAsync(id, userId, eventCard);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("event-card/{id}")]
        public async Task<ActionResult> DeleteEventCard(string id)
        {
            try
            {
                var result = await _eventCardService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
