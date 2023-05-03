using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Please enter your token")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(6, ErrorMessage = "Please enter at least 6 character")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your confirm password")]
        [Compare("Password", ErrorMessage = "Your password and confirm password must be the same")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
