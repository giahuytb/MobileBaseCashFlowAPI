namespace MobileBasedCashFlowAPI.DTO
{
    public class GameMatchRequest
    {
        public int MaxNumberPlayer { get; set; }
        public int? WinnerId { get; set; }
        public int? LastHostId { get; set; }
        public int TotalRound { get; set; }
    }
}
