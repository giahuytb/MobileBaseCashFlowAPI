
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Services;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipsController : ControllerBase
    {
        private readonly IFriendShipService _friendShipService;

        public FriendShipsController(IFriendShipService friendShipService)
        {
            _friendShipService = friendShipService;
        }

        [HttpGet("friend-ship")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _friendShipService.GetAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("friend-ship/{requester}&{addresee}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetFriendShipById(string requester, string addresee)
        {
            try
            {
                var result = await _friendShipService.GetAsync(requester, addresee);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("friend-ship")]
        public async Task<ActionResult> PostFriendShip(string addressee)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _friendShipService.AddFriendShip(userId, addressee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
