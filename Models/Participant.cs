using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class Participant
    {
        public string UserId { get; set; } = null!;
        public string MatchId { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual GameMatch Match { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
    }
}
