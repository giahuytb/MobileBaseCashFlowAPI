using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Item
    {
        public Item()
        {
            Inventories = new HashSet<Inventory>();
        }

        public string ItemId { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public string ItemImageUrl { get; set; } = null!;
        public double ItemPrice { get; set; }
        public string Description { get; set; } = null!;
        public bool IsInShop { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public byte? ItemType { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
