using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Please enter your user name")]
        [MaxLength(30, ErrorMessage = "Do not enter more than 20 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string Password { get; set; } = string.Empty;

    }
}
