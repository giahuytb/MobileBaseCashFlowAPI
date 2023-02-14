using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class Inventory
    {
        public string UserId { get; set; } = null!;
        public string ItemId { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual UserAccount User { get; set; } = null!;
    }
}
