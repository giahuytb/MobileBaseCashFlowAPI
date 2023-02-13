using System;
using System.Collections.Generic;

namespace MobieBaseCashFlowAPI.Models
{
    public partial class Leaderboard
    {
        public string LeaderBoardId { get; set; } = null!;
        public byte TimeFeriod { get; set; }
        public DateTime TimePeriodFrom { get; set; }
        public int Score { get; set; }
        public DateTime CreateAt { get; set; }
        public string? GameId { get; set; }
        public string? PlayerId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual UserAccount? Player { get; set; }
    }
}
