namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class FinancialRequest
    {
        public string Job_card_id { get; set; } = string.Empty;
        public int Children_amount { get; set; } = 0;
        public List<GameAccountRequest> Game_accounts { get; set; } = new List<GameAccountRequest>();
    }
}
