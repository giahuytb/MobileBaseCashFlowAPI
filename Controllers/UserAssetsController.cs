
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using Microsoft.AspNetCore.Authorization;
using MobileBasedCashFlowAPI.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssetsController : ControllerBase
    {
        private readonly UserAssetRepository _userAssetRepository;

        public UserAssetsController(UserAssetRepository inventoryService)
        {
            _userAssetRepository = inventoryService;
        }

        ////[Authorize(Roles = "Player, Admin")]
        //[HttpGet]
        //[SwaggerOperation(Summary = "Get all user asset")]
        //public async Task<ActionResult<IEnumerable>> GetALl()
        //{
        //    var result = await _userAssetRepository.GetAsync();
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound("List is empty");
        //}


        [Authorize(Roles = "Player, Admin")]
        [HttpGet("my-asset")]
        [SwaggerOperation(Summary = "Get all assets owned by the player (Login require)")]
        public async Task<ActionResult<UserAsset>> GetMyAsset()
        {
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.GetAsync(Int32.Parse(userId));
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this user inventory ");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Buy an asset (Login require)")]
        public async Task<ActionResult> BuyAsset([FromBody] int assetId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.CreateAsync(assetId, Int32.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpPut("asset-last-used")]
        [SwaggerOperation(Summary = "Update last use asset (Login require)")]
        public async Task<ActionResult> UpdateAssetLastUsed(int assetId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.UpdateLastUsedAsync(assetId, Int32.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
