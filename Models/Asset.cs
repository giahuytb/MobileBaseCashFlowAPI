using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Asset
    {
        public Asset()
        {
            UserAssets = new HashSet<UserAsset>();
        }

        public int AssetId { get; set; }
        public string AssetName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public double AssetPrice { get; set; }
        public string Description { get; set; } = null!;
        public bool IsInShop { get; set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public int? AssetType { get; set; }

        public virtual AssetType? AssetTypeNavigation { get; set; }
        public virtual ICollection<UserAsset> UserAssets { get; set; }
    }
}
