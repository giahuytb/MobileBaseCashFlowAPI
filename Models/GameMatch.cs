using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameMatch
    {
        public GameMatch()
        {
            Participants = new HashSet<Participant>();
        }

        public string MatchId { get; set; } = null!;
        public int MaxNumberPlayer { get; set; }
        public string? WinnerId { get; set; }
        public string? HostId { get; set; }
        public string? LastHostId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalRound { get; set; }
        public string? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual UserAccount? Host { get; set; }
        public virtual UserAccount? LastHost { get; set; }
        public virtual UserAccount? Winner { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
