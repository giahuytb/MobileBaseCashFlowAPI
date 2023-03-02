namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class JobCardRequest
    {
        public string Job_card_name { get; set; } = string.Empty;
        public int Children_cost { get; set; } = 0;
        public List<GameAccountRequest> Game_accounts { get; set; } = new List<GameAccountRequest>();
    }
}
