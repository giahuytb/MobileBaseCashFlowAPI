using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class JobAccount
    {
        public string JobCardId { get; set; } = null!;
        public string GameAccountId { get; set; } = null!;
        public double? Value { get; set; }

        public virtual GameAccount GameAccount { get; set; } = null!;
        public virtual JobCard JobCard { get; set; } = null!;
    }
}
