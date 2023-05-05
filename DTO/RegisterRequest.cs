using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Please enter your user name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password")]
        [MinLength(6, ErrorMessage = "Please enter at least 6 character")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your confirm password")]
        [Compare("Password", ErrorMessage = "Your password and confirm password must be the same")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "PLease Enter the correct Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
