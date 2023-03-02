namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class GameAccountRequest
    {
        public string Game_account_name { get; set; } = null!;
        public int Game_account_type { get; set; }
        public double Game_account_value { get; set; }
    }
}
