using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class AssetType
    {
        public AssetType()
        {
            Assets = new HashSet<Asset>();
        }

        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; } = null!;

        public virtual ICollection<Asset> Assets { get; set; }
    }
}
