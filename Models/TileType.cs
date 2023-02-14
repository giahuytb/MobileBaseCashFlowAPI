using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class TileType
    {
        public TileType()
        {
            Tiles = new HashSet<Tile>();
        }

        public string TileTypeId { get; set; } = null!;
        public string TileTypeName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
