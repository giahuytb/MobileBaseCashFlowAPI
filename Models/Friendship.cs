using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Friendship
    {
        public Friendship()
        {
            FriendshipStatuses = new HashSet<FriendshipStatus>();
        }
        public string RequesterId { get; set; } = null!;
        public string AddresseeId { get; set; } = null!;
        public DateTime CreateAt { get; set; }

        public virtual UserAccount Addressee { get; set; } = null!;
        public virtual UserAccount Requester { get; set; } = null!;
        public virtual ICollection<FriendshipStatus> FriendshipStatuses { get; set; }
    }
}
