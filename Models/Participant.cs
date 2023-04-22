using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Participant
    {
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public DateTime CreateAt { get; set; }

        public virtual GameMatch Match { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
    }
}
