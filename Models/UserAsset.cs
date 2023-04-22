using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class UserAsset
    {
        public int UserId { get; set; }
        public int AssetId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? LastUsed { get; set; }

        public virtual Asset Asset { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
    }
}
