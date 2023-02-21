namespace MobileBasedCashFlowAPI.DTO
{
    public class LeaderboardRequest
    {
        public byte TimePeriod { get; set; }
        public DateTime TimePeriodFrom { get; set; }
        public int Score { get; set; }
        public string GameVersion { get; set; } = null!;
    }
}
