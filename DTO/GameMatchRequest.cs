namespace MobileBasedCashFlowAPI.DTO
{
    public class GameMatchRequest
    {
        public int MaxNumberPlayer { get; set; }
        public string? WinnerId { get; set; }
        public string? LastHostId { get; set; }
        public int TotalRound { get; set; }
    }
}
