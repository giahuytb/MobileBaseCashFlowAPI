using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class FriendshipStatus
    {
        public string RequesterId { get; set; } = null!;
        public string AddresseeId { get; set; } = null!;
        public DateTime SpecifiedDateTime { get; set; }
        public string StatusCode { get; set; } = null!;
        public string SpecifierId { get; set; } = null!;

        public virtual Friendship Friendship { get; set; } = null!;
        public virtual UserAccount Specifier { get; set; } = null!;
    }
}
