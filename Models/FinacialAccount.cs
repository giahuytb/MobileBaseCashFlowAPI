using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class FinacialAccount
    {
        public string FinacialId { get; set; } = null!;
        public string GameAccountId { get; set; } = null!;
        public double? Value { get; set; }

        public virtual FinacialReport Finacial { get; set; } = null!;
        public virtual GameAccount GameAccount { get; set; } = null!;
    }
}
