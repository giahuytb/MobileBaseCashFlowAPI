using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class AccountRequest
    {
        [Required(ErrorMessage = "Please enter your game account name")]
        [MaxLength(50, ErrorMessage = "Game account name is too long (max is 50)")]
        public string Game_account_name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your game account type")]
        [MaxLength(50, ErrorMessage = "Game account type is too long (max is 50)")]
        public string Game_account_type { get; set; } = null!;
    }
}
