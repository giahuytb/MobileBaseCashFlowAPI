using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class AccountRequest
    {
        [Required(ErrorMessage = "Please enter your game account name")]
        public string Game_account_name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your game account type")]
        public string Game_account_type { get; set; } = null!;
    }
}
