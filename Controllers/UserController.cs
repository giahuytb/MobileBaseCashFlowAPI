using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Services;
using System.Collections;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.Authenticate(request);

            if (result.Equals("User not found"))
            {
                return BadRequest("Can not found your account.");
            }
            else if (result.Equals("Wrong password"))
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
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _userService.Register(request);
                if (result.Equals("success"))
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            try
            {
                var result = await _userService.VerifyEmail(token);
                if (result.Equals("success"))
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var result = await _userService.ForgotPassword(email);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                var result = await _userService.ResetPassword(request);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _userService.GetAsync();
                if (result == null)
                {
                    return NotFound("list is empty");
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
