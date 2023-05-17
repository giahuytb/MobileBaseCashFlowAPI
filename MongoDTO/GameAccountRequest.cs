using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class GameAccountRequest
    {
        [Required(ErrorMessage = "Please enter game account name")]
        public string Game_account_name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter game account type")]
        public string Game_account_type { get; set; } = null!;

        [Required(ErrorMessage = "Please enter game account value")]
        [Range(0, double.MaxValue, ErrorMessage = "Game account value must be mumber and bigger than or equal to 0")]
        public double Game_account_value { get; set; }

        [Required(ErrorMessage = "Please enter amount for this game account")]
        [Range(0, int.MaxValue, ErrorMessage = "Amount must be mumber and bigger than or equal to 0")]
        public int Amount { get; set; }
    }
}
