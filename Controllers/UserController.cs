using MobileBaseCashFlowGameAPI.Common;
using MobileBaseCashFlowGameAPI.IServices;
using MobileBaseCashFlowGameAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileBaseCashFlowGameAPI.Controllers
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

            if (result.Equals("User Not Found"))
            {
                return BadRequest("Can not found your account.");
            }
            else if (result.Equals("Wrong Password"))
            {
                return BadRequest("Your password is not correct, please try again.");
            }
            else
            {
                return Ok(new { StatusCode = 200, Message = "Login Successfully", data = result });
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
                if (result.Equals("Success"))
                {
                    return Ok(new { StatusCode = 201, Message = result });
                }
                return BadRequest(new { StatusCode = 404, result });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { StatusCode = 404, Message = "Somthing is wrong" });
            }
        }

        [AllowAnonymous]
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            try
            {
                var result = await _userService.VerifyEmail(token);
                if (result.Equals("Success"))
                {
                    return Ok(new { StatusCode = 200, MessagePack = "Verify Email successfully" });
                }
                else
                {
                    return BadRequest(new { StatusCode = 400, Message = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
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
                    return Ok(new { StatusCode = 200, MessagePack = "Send Mail To Reset Password Success" });
                }
                else
                {
                    return BadRequest(new { StatusCode = 400, Message = result });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
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
                    return Ok(new { StatusCode = 200, MessagePack = "Reset password successfully" });
                }
                else
                {
                    return BadRequest(new { StatusCode = 400, Message = "Invalid token" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, Message = ex.ToString() });
            }
        }
    }
}
