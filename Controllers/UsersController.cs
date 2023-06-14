using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Dto;
using System.Collections;
using MobileBasedCashFlowAPI.Utils;
using Swashbuckle.AspNetCore.Annotations;
using MobileBasedCashFlowAPI.Models;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly MobileBasedCashFlowGameContext _context;

        public UsersController(IUserRepository userService, MobileBasedCashFlowGameContext context)
        {
            _userService = userService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [SwaggerOperation(Summary = "Login for get information and access token")]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Authenticate(request);

            if (result.Equals(Constant.NotFound))
            {
                return NotFound("Can not found your account.");
            }
            else if (result.Equals(Constant.WrongPassword))
            {
                return BadRequest("Your password is not correct, please try again.");
            }
            else
            {
                return Ok(result);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [SwaggerOperation(Summary = "register an account")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Register(request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("profile/{userId}")]
        [Authorize(Roles = "Player, Admin")]
        [SwaggerOperation(Summary = "Edit the profile")]
        public async Task<IActionResult> EditProfile(int userId, EditProfileRequest request)
        {
            var result = await _userService.EditProfile(userId, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("user-list")]
        [SwaggerOperation(Summary = "Get all list user")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.GetAllAsync();
            if (result == null)
            {
                return NotFound("List is empty");
            }
            return Ok(result);
        }

        [HttpPut("coin-point/{userId}")]
        [SwaggerOperation(Summary = "Update coin and point, coin, point input will be added to the existing coin, point)")]
        public async Task<IActionResult> AddCoinToUser(int userId, EditRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateCoin(userId, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("id")]
        [SwaggerOperation(Summary = "Find user by using id")]
        public async Task<ActionResult<IEnumerable>> FindUserById(int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.FindUserById(userId);
            if (result == null)
            {
                return NotFound("Can not find this user");
            }
            return Ok(result);
        }

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
            var result = await _userService.GetUserAsset(int.Parse(userId));
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this user inventory ");
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("buy")]
        [SwaggerOperation(Summary = "Buy an asset (Login require)")]
        public async Task<ActionResult> BuyAsset([FromForm] int assetId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userService.BuyAsset(assetId, int.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpPut("asset-last-used/{userId}")]
        [SwaggerOperation(Summary = "Update last use asset (Login require)")]
        public async Task<ActionResult> UpdateAssetLastUsed(int userId, LastUsedRequest request)
        {
            var result = await _userService.UpdateLastUsed(userId, request);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpDelete("delete-asset")]
        [SwaggerOperation(Summary = "Delete your asset (Login require)")]
        public async Task<ActionResult> DeleteAsset(int assetId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userService.DeleteMyAsset(assetId, int.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
