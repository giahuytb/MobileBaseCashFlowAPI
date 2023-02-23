
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventorysController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventorysController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _inventoryService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //[HttpGet]
        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("inventory/{searchBy}&{id}")]
        public async Task<ActionResult<Inventory>> GetById(string searchBy, string id)
        {
            try
            {
                var result = await _inventoryService.GetAsync(searchBy, id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not find iventory of this " + searchBy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("inventory")]
        public async Task<ActionResult> BuyItem(string itemId)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _inventoryService.CreateAsync(itemId, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
