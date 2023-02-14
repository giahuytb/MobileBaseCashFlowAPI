using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class GameMatch
    {
        public GameMatch()
        {
            Users = new HashSet<UserAccount>();
        }

        public string MatchId { get; set; } = null!;
        public int MaxNumberPlayer { get; set; }
        public string? WinerId { get; set; }
        public string? HostId { get; set; }
        public string? LastHostId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalRound { get; set; }
        public string? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual UserAccount? Host { get; set; }
        public virtual UserAccount? LastHost { get; set; }
        public virtual UserAccount? Winer { get; set; }

        public virtual ICollection<UserAccount> Users { get; set; }
    }
}
