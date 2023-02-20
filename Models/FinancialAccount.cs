using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class FinancialAccount
    {
        public string FinacialId { get; set; } = null!;
        public string GameAccountId { get; set; } = null!;
        public double? Value { get; set; }

        public virtual FinancialReport Finacial { get; set; } = null!;
        public virtual GameAccount GameAccount { get; set; } = null!;
    }
}
