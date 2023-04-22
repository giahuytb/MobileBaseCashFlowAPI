
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssetsController : ControllerBase
    {
        private readonly UserAssetRepository _inventoryService;

        public UserAssetsController(UserAssetRepository inventoryService)
        {
            _inventoryService = inventoryService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _inventoryService.GetAsync();
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


        [Authorize(Roles = "Player, Admin")]
        [HttpGet("my-asset")]
        public async Task<ActionResult<UserAsset>> GetById()
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                string users = HttpContext.User.FindFirstValue("roleName");
                if (userId == null)
                {
                    return Unauthorized("User id not Found, please login");
                }
                var result = await _inventoryService.GetAsync(Int32.Parse(userId));
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("Can not found this user iventory ");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<ActionResult> BuyItem(int itemId)
        {
            try
            {
                // get the id of current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return Unauthorized("User id not Found, please login");
                }
                var result = await _inventoryService.CreateAsync(itemId, Int32.Parse(userId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
