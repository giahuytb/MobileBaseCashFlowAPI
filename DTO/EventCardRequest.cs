namespace MobileBasedCashFlowAPI.DTO
{
    public class EventCardRequest
    {
        public string CardName { get; set; } = null!;
        public double Cost { get; set; }
        public double DownPay { get; set; }
        public double Dept { get; set; }
        public double CashFlow { get; set; }
        public string TradingRange { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string EventImageUrl { get; set; } = null!;
        public string GameEventName { get; set; } = null!;
    }
}
