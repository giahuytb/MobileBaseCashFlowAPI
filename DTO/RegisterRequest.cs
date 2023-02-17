using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.DTO
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
