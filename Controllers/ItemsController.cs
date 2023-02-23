using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.DTO;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("item")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _itemService.GetAsync();
            return Ok(result);
        }

        [HttpGet("item/{id}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<Item>> GetById(string name)
        {
            try
            {
                var result = await _itemService.GetAsync(name);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("item")]
        public async Task<ActionResult> PostItem(ItemRequest item)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _itemService.CreateAsync(userId, item);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("item/{id}")]
        public async Task<ActionResult> UpdateItem(string id, ItemRequest item)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _itemService.UpdateAsync(id, userId, item);
                if (result != "success")
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("item/{id}")]
        public async Task<ActionResult> DeleteItem(string id)
        {
            try
            {
                var result = await _itemService.DeleteAsync(id);
                if (result != "success")
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
