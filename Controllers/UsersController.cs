using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.DTO;
using System.Collections;
using MobileBasedCashFlowAPI.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userService;

        public UsersController(UserRepository userService)
        {
            _userService = userService;
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

        [AllowAnonymous]
        [HttpGet("verify-email")]
        [SwaggerOperation(Summary = "verify email with token")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.VerifyEmail(token);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        [SwaggerOperation(Summary = "use mail to get reset password code in mail")]
        public async Task<IActionResult> ForgotPassword(string userName, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ForgotPassword(userName, email);
            if (result)
            {
                return Ok(Constant.Success);
            }
            else
            {
                return BadRequest(Constant.Failed);
            }
        }


        [HttpPut("profile")]
        [Authorize(Roles = "Player, Admin, Moderator")]
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
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        [SwaggerOperation(Summary = "Change your password after get code in mail")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.ResetPassword(request);
            if (result)
            {
                return Ok(Constant.Success);
            }
            else
            {
                return BadRequest(Constant.Failed);
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
            var result = await _userService.GetAsync();
            if (result == null)
            {
                return NotFound("List is empty");
            }
            return Ok(result);
        }

        [HttpPut("coin")]
        [SwaggerOperation(Summary = "Update coin, coin input will be added to the existing coin)")]
        public async Task<IActionResult> AddCoinToUser(int userId, int coin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.UpdateCoin(userId, coin);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(Constant.Failed);
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



    }
}
