using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class EventCard
    {
        public string EventCardId { get; set; } = null!;
        public string CardName { get; set; } = null!;
        public double Cost { get; set; }
        public double DownPay { get; set; }
        public double Dept { get; set; }
        public double CashFlow { get; set; }
        public string TradingRange { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public string? GameId { get; set; }
        public string? EventId { get; set; }
        public string EventImageUrl { get; set; } = null!;

        public virtual GameEvent? Event { get; set; }
        public virtual Game? Game { get; set; }
    }
}
