using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameMatch
    {
        public GameMatch()
        {
            GameReports = new HashSet<GameReport>();
            Participants = new HashSet<Participant>();
        }

        public string MatchId { get; set; } = null!;
        public int MaxNumberPlayer { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int TotalRound { get; set; }
        public int? WinnerId { get; set; }
        public int? HostId { get; set; }
        public int? LastHostId { get; set; }
        public int? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual UserAccount? Host { get; set; }
        public virtual UserAccount? LastHost { get; set; }
        public virtual UserAccount? Winner { get; set; }
        public virtual ICollection<GameReport> GameReports { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
