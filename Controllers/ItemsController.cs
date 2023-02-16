using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobieBasedCashFlowAPI.IServices;
using MobieBasedCashFlowAPI.Models;
using MobieBasedCashFlowAPI.ViewModels;
using System.Collections;
using System.Security.Claims;

namespace MobieBasedCashFlowAPI.Controllers
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
        public async Task<ActionResult<IEnumerable>> Get()
        {
            var result = await _itemService.GetAsync();

            return Ok(result);
        }

        [HttpGet("item/{name}")]
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
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("item")]
        public async Task<ActionResult> PostItem(itemRequest item)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found , please login again ");
                }
                var item1 = await _itemService.CreateAsync(userId, item);

                return CreatedAtAction(nameof(GetById), new { name = item.ItemName }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("item")]
        public async Task<ActionResult> UpdateItem(string id, itemRequest item)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found , please login again ");
                }
                var result = await _itemService.UpdateAsync(id, userId, item);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("item")]
        public async Task<ActionResult> DeleteItem(string id)
        {
            try
            {
                var result = await _itemService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
