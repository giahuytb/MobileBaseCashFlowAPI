
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _assetRepository;

        public AssetsController(IAssetRepository itemService)
        {
            _assetRepository = itemService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all asset")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _assetRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("user/asset-in-shop")]
        [SwaggerOperation(Summary = "Get all asset that player still not buy in shop (Login require)")]
        public async Task<ActionResult<IEnumerable>> GetAssetInShop()
        {
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _assetRepository.GetAssetInShop(Int32.Parse(userId));
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get asset by asset id")]
        public async Task<ActionResult<Asset>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _assetRepository.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this asset");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new asset")]
        public async Task<ActionResult> PostAsset(AssetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _assetRepository.CreateAsync(Int32.Parse(userId), request);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing asset")]
        public async Task<ActionResult> UpdateAsset(int id, AssetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _assetRepository.UpdateAsync(id, Int32.Parse(userId), request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing asset")]
        public async Task<ActionResult> DeleteAsset(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _assetRepository.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
